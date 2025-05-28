using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] PathWaypoint startWaypoint;
    PathWaypoint currentWaypoint;
    PathWaypoint prevWaypoint;
    List<bool> pastDirections;
    bool generateRight;
    Dictionary<List<PathWaypoint>, CapsuleCollider> waypointsPerArea;
    List<CapsuleCollider> unvisitedAreas;

    public static event Action OnPathGenerated;

    private void Awake()
    {
        PathCategorizeWaypoint.OnWaypointsCategorized += GetWaypointsInArea;
        ShareAreas.OnAreaShare += GetAreas;

        pastDirections = new List<bool>();
        waypointsPerArea = new Dictionary<List<PathWaypoint>, CapsuleCollider>();
        unvisitedAreas = new List<CapsuleCollider>();
    }

    void Start()
    {
        // This sets the previous waypoint to south, which allows the path to start generating properly.
        prevWaypoint = startWaypoint.waypoints.Keys[1];
        currentWaypoint = startWaypoint;
    }

    private void Update()
    {
        // We stop generating if the current waypoint is the end one or the path manages to double back on itself and reach an already visited waypoint.
        // We also don't generate yet if the waypoints per area doesn't match the total number of areas.
        // Othwerwise  we decide what the next waypoint should be.
        if (!currentWaypoint.visited && waypointsPerArea.Count != 0) { GeneratePath(); }
        //else { Debug.Log("Generation ended at " + currentWaypoint); }
    }

    void GeneratePath()
    {
        Debug.Log("Generating for waypoint: " + currentWaypoint.name);
        // Mark a waypoint as visited.
        currentWaypoint.visited = true;

        // Check if we've visited a new area
        CheckAreaVisited(currentWaypoint);

        //Determine which waypoints are left and right based on which direction the path is coming from.
        if (prevWaypoint != null)
        {
            //This will have the waypoints that the next one will be chosen from.
            UDictionary<PathWaypoint, bool> possibleNextWaypoints = new UDictionary<PathWaypoint, bool>();
            bool swapResults = false;
            for (int i = 0; i < currentWaypoint.waypoints.Count; i++)
            {
                if (prevWaypoint == currentWaypoint.waypoints.Keys[i])
                {
                    //Make the previous path section visible.
                    EnablePathSection(i);

                    // If it the final waypoint we don't need to check for the next one.
                    if(currentWaypoint.endWaypoint) 
                    {
                        Debug.Log("Unvisited areas: " + unvisitedAreas.Count);

                        //Hide all waypoint pieces.
                        for (int j = 0; j < transform.childCount; j++) { transform.GetChild(j).gameObject.SetActive(false); }

                        // Let the object spawner know that path generation has finished.
                        if (OnPathGenerated != null) { OnPathGenerated(); }
                        return; 
                    }

                    //Swap the results of left and right if the waypoints orientation is North of East, so it matches their viewpoint.
                    if (prevWaypoint == currentWaypoint.waypoints.Keys[0] || prevWaypoint == currentWaypoint.waypoints.Keys[3]) { swapResults = true;
                        //Debug.Log("Swapping results!");
                    }

                    // We set the i, so that it adds the waypoints perpendicular to the direction the path is coming from.
                    if (i <= 1){ i = 2; }
                    else{ i = 0; }
                    possibleNextWaypoints.Add(currentWaypoint.waypoints.Keys[i], currentWaypoint.waypoints.Values[i]);
                    possibleNextWaypoints.Add(currentWaypoint.waypoints.Keys[i+1], currentWaypoint.waypoints.Values[i+1]);

                    //Remove current waypoint and possible waypoints from the original dict. This will allow the option to add a backup option if necessary.
                    currentWaypoint.waypoints.Keys.Remove(prevWaypoint);
                    currentWaypoint.waypoints.Remove(possibleNextWaypoints.Keys[0]);
                    currentWaypoint.waypoints.Remove(possibleNextWaypoints.Keys[1]);
                    break;
                }
            }
            //Check if one of the possible waypoints is missing.
            for (int i = 0; i < possibleNextWaypoints.Count; i++)
            {
                if (possibleNextWaypoints.Keys[i] == null)
                {
                    //Debug.LogFormat("Replacing missing waypoint at index {0} with {1}", i, currentWaypoint.waypoints.Keys[0]);
                    //If it is missing, replace it with the one remaining waypoint in the original dict. If there are no re 
                    possibleNextWaypoints.Keys[i] = currentWaypoint.waypoints.Keys[0];
                    possibleNextWaypoints.Values[i] = currentWaypoint.waypoints.Values[0];
                    break;
                }
            }

            //Debug.LogFormat("Possible waypoints {0} and {1}.", possibleNextWaypoints.Keys[0].name, possibleNextWaypoints.Keys[1].name);
            if (swapResults) { DecideNextWaypoint(new KeyValuePair<PathWaypoint, bool>(possibleNextWaypoints.Keys[1], possibleNextWaypoints.Values[1]), new KeyValuePair<PathWaypoint, bool>(possibleNextWaypoints.Keys[0], possibleNextWaypoints.Values[0])); }
            else { DecideNextWaypoint(new KeyValuePair<PathWaypoint, bool>(possibleNextWaypoints.Keys[0], possibleNextWaypoints.Values[0]), new KeyValuePair<PathWaypoint, bool>(possibleNextWaypoints.Keys[1], possibleNextWaypoints.Values[1])); }
        }
        else { DecideNextWaypoint(new KeyValuePair<PathWaypoint, bool>(currentWaypoint.waypoints.Keys[2], currentWaypoint.waypoints.Values[2]), new KeyValuePair<PathWaypoint, bool>(currentWaypoint.waypoints.Keys[3], currentWaypoint.waypoints.Values[3])); ; }
    }

    void DecideNextWaypoint(KeyValuePair<PathWaypoint, bool> leftPair, KeyValuePair<PathWaypoint, bool> rightPair)
    {
        //Debug.LogFormat("Choosing next waypoint between {0} and {1}.", leftPair.Key.name, rightPair.Key.name);

        //If one of the options has already been visited or the path is disabled, the direction needs to be forced the other way.
        //Path is also forced if the direction has no valid waypoints that can be taken next.
        if (leftPair.Key.visited || !leftPair.Value || !CheckForValidWaypoints(leftPair.Key))
        {
            Debug.LogFormat("Forcing direction right. Visited: {0} Path status: {1}", leftPair.Key.visited, leftPair.Value);
            generateRight = true;
        }
        else if (rightPair.Key.visited || !rightPair.Value || !CheckForValidWaypoints(rightPair.Key))
        {
            Debug.LogFormat("Forcing direction left. Visited: {0} Path status: {1}", rightPair.Key.visited, rightPair.Value);
            generateRight = false;
        }
        // Special case where the path reaches the end but hasn't visited all areas yet.
        else if (leftPair.Key.endWaypoint && unvisitedAreas.Count != 0)
        {
            Debug.Log("End reached but not all areas have been visited. Redirecting right.");
            generateRight = true;
            // Allows the second to last waypoint to be reached again.
            currentWaypoint.visited = false;
        }
        else if (rightPair.Key.endWaypoint && unvisitedAreas.Count != 0)
        {
            Debug.Log("End reached but not all areas have been visited. Redirecting left.");
            generateRight = false;
            currentWaypoint.visited = false;
        }
        //If the direction isn't forced we randomise it.
        else
        {
            generateRight = RandomisePathDirection();
            Debug.Log("Going right? " + generateRight);
        }

        // Add the direction to the list after any potential forcing, so the accurate version gets added.
        pastDirections.Add(generateRight);

        prevWaypoint = currentWaypoint;
        if (generateRight) { currentWaypoint = rightPair.Key; }
        else { currentWaypoint = leftPair.Key; }
        // Disable previous waypoint to note that it's already been visited.
        prevWaypoint.enabled = false;
    }

    bool RandomisePathDirection()
    {
        // Restrict direction if the past two directions have been the same.
        if (pastDirections.Count >= 2 && pastDirections[pastDirections.Count - 1] == pastDirections[pastDirections.Count - 2])
        {
            Debug.Log("2 of the same results in a row, forcing direction.");
            // Force the path to go in the opposite direction than the past two directions.
            return !pastDirections[pastDirections.Count - 1];
        }
        else
        {
            int randomNumber = UnityEngine.Random.Range(0, 2);
            if (randomNumber == 0) { return true; }
            else { return false; }
        }
    }

    void EnablePathSection(int pathIndex)
    {
        // Activate the path that corresponds to the direction the path came from.
        //Debug.LogFormat("Enabling path at index {0} for {1}", pathIndex, currentWaypoint);
        switch (pathIndex)
        {
            case 0:
                if (currentWaypoint.northPath != null) { currentWaypoint.northPath.SetActive(true); }
                break;

            case 1:
                if (currentWaypoint.southPath != null) { currentWaypoint.southPath.SetActive(true); }
                break;

            case 2:
                if (currentWaypoint.westPath != null) { currentWaypoint.westPath.SetActive(true); }
                break;

            case 3:
                if (currentWaypoint.eastPath != null) { currentWaypoint.eastPath.SetActive(true); }
                break;

            default:
                break;
        }
    }

    bool CheckForEndWaypoint()
    {
        for (int i = 0; i < currentWaypoint.waypoints.Count; i++)
        {
            if(currentWaypoint.waypoints.Keys[i] != null && currentWaypoint.waypoints.Keys[i].endWaypoint && unvisitedAreas.Count == 0)
            {
                //if the end waypoint is found, force that to be the next waypoint.
                DecideNextWaypoint(new KeyValuePair<PathWaypoint, bool>(currentWaypoint.waypoints.Keys[i], currentWaypoint.waypoints.Values[i]), new KeyValuePair<PathWaypoint, bool>(currentWaypoint.waypoints.Keys[i], currentWaypoint.waypoints.Values[i]));
                return true;
            }
        }
        return false;
    }

    bool CheckForValidWaypoints(PathWaypoint waypointToCheck)
    {
        for (int i = 0; i < waypointToCheck.waypoints.Count; i++)
        {
            if (waypointToCheck.waypoints.Keys[i] != null && !waypointToCheck.waypoints.Keys[i].visited)
            {
                //Debug.LogFormat("Waypoint {0} has a valid waypoint with {1}.", waypointToCheck, waypointToCheck.waypoints.Keys[i].name);
                //If a waypoint is valid AKA the waypoint isn't null and hasn't been visited yet, we return true.
                return true;
            }
        }
        //Debug.LogFormat("Waypoint {0} does NOT have a valid waypoint!", waypointToCheck);
        return false;
    }

    void RemoveWaypointFromArea()
    {
        //Waypoints are removed from areas
    }

    void GetWaypointsInArea(List<PathWaypoint> waypoints, CapsuleCollider area)
    {
        waypointsPerArea.Add(waypoints, area);
    }

    void GetAreas(List<CapsuleCollider> areas)
    {
        unvisitedAreas = areas;
        Debug.Log("Amount of unvisited areas: " + unvisitedAreas.Count);
    }

    void CheckAreaVisited(PathWaypoint waypoint)
    {
        foreach(List<PathWaypoint> waypoints in waypointsPerArea.Keys) 
        {
            // If the waypoint is in an area that means we've visited that area
            if (waypoints.Contains(waypoint))
            {
                CapsuleCollider currentArea;
                waypointsPerArea.TryGetValue(waypoints, out currentArea);

                if(unvisitedAreas.Contains(currentArea))
                {
                    unvisitedAreas.Remove(currentArea);
                    Debug.Log("Visited area " + currentArea);
                    break;
                }
            }
        }
    }

    private void OnDestroy()
    {
        PathCategorizeWaypoint.OnWaypointsCategorized -= GetWaypointsInArea;
        ShareAreas.OnAreaShare -= GetAreas;
    }
}
