using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] PathWaypoint startWaypoint;
    PathWaypoint currentWaypoint;
    PathWaypoint prevWaypoint;
    List<bool> pastDirections;
    bool generateRight;

    [SerializeField] bool debugMode = false;
    MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        //Hide all waypoint pieces before generating. They are then revealed if they are part of the main path.
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        pastDirections = new List<bool>();
        // This sets the previous waypoint to south, which allows the path to start generating properly.
        prevWaypoint = startWaypoint.waypoints.Keys[1];
        currentWaypoint = startWaypoint;
        //GeneratePath(startWaypoint);
    }

    private void Update()
    {
        // We stop generating if the current waypoint is the end one or the path manages to double back on itself and reach an already visited waypoint.
        // Othwerwise  we decide what the next waypoint should be.
        if (!currentWaypoint.visited) { GeneratePath(); }
        //else { Debug.Log("Generation ended at " + currentWaypoint); }
    }

    void GeneratePath()
    {
        //Debug.Log("Generating for waypoint: " + currentWaypoint.name);
        // Mark a waypoint as visited.
        if(debugMode)
        {
            currentWaypoint.gameObject.SetActive(true);
            mr = currentWaypoint.GetComponent<MeshRenderer>();
            mr.enabled = true;
        }
        currentWaypoint.visited = true;

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
                    if(currentWaypoint.endWaypoint) { return; }

                    //Swap the results of left and right if the waypoints orientation is North of East, so it matches their viewpoint.
                    if (prevWaypoint == currentWaypoint.waypoints.Keys[0] || prevWaypoint == currentWaypoint.waypoints.Keys[3]){ swapResults = true; }

                    // We set the i, so that it adds the waypoints perpendicular to the direction the path is coming from.
                    if (i <= 1) { i = 2; }
                    else { i = 0; }
                    possibleNextWaypoints.Add(currentWaypoint.waypoints.Keys[i], currentWaypoint.waypoints.Values[i]);
                    possibleNextWaypoints.Add(currentWaypoint.waypoints.Keys[i + 1], currentWaypoint.waypoints.Values[i + 1]);

                    //Remove current waypoint and possible waypoints from the original dict. This will allow the option to add a backup option if necessary.
                    currentWaypoint.waypoints.Keys.Remove(prevWaypoint);

                    //Check if one of the waypoints is the ending one, we need to go to that one, so the rest can be skipped.
                    if(CheckForEndWaypoint()){ return; }

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
    }

    void DecideNextWaypoint(KeyValuePair<PathWaypoint, bool> leftPair, KeyValuePair<PathWaypoint, bool> rightPair)
    {
        //Debug.LogFormat("Choosing next waypoint between {0} and {1}.", leftPair.Key.name, rightPair.Key.name);

        // If one of the options has already been visited or the path is disabled, the direction needs to be forced the other way.
        if (leftPair.Key.visited || !leftPair.Value)
        {
            //Debug.LogFormat("Forcing direction right. Visited: {0} Path status: {1}", leftPair.Key.visited, leftPair.Value);
            generateRight = true;
        }
        else if (rightPair.Key.visited || !rightPair.Value)
        {
            //Debug.LogFormat("Forcing direction left. Visited: {0} Path status: {1}", rightPair.Key.visited, rightPair.Value);
            generateRight = false;
        }
        // If the direction isn't forced we randomise it.
        else
        {
            generateRight = RandomisePathDirection();
            //Debug.Log("Going right? " + generateRight);
        }

        // Add the direction to the list after any potential forcing, so the accurate version gets added.
        pastDirections.Add(generateRight);
        // If Debug is enabled, change the material to add a visual indication of which direction the path generated
        if (debugMode && generateRight) { mr.material.color = Color.green; }

        prevWaypoint = currentWaypoint;
        if (generateRight) { currentWaypoint = rightPair.Key; }
        else { currentWaypoint = leftPair.Key; }
    }

    bool RandomisePathDirection()
    {
        // Restrict direction if the past two directions have been the same.
        if (pastDirections.Count >= 2 && pastDirections[pastDirections.Count - 1] == pastDirections[pastDirections.Count - 2])
        {
            //Debug.Log("2 of the same results in a row, forcing direction.");
            // Force the path to go in the opposite direction than the past two directions.
            return !pastDirections[pastDirections.Count - 1];
        }
        else
        {
            int randomNumber = Random.Range(0, 2);
            if (randomNumber == 0) { return true; }
            else { return false; }
        }
    }

    void EnablePathSection(int pathIndex)
    {
        // Activate the path that corresponds to the direction the path came from.
        switch (pathIndex)
        {
            case 0:
                if (currentWaypoint.northPath != null)
                {
                    //Debug.LogFormat("Enabling north path for {0}", currentWaypoint);
                    currentWaypoint.northPath.SetActive(true);
                }
                break;

            case 1:
                if (currentWaypoint.southPath != null)
                {
                    //Debug.LogFormat("Enabling south path for {0}", currentWaypoint);
                    currentWaypoint.southPath.SetActive(true);
                }
                break;

            case 2:
                if (currentWaypoint.westPath != null)
                {
                    //Debug.LogFormat("Enabling west path for {0}", currentWaypoint);
                    currentWaypoint.westPath.SetActive(true);
                }
                break;

            case 3:
                if (currentWaypoint.eastPath != null)
                {
                    //Debug.LogFormat("Enabling east path for {0}", currentWaypoint);
                    currentWaypoint.eastPath.SetActive(true);
                }
                break;

            default:
                break;
        }
    }

    bool CheckForEndWaypoint()
    {
        for (int i = 0; i < currentWaypoint.waypoints.Count; i++)
        {
            if(currentWaypoint.waypoints.Keys[i] != null && currentWaypoint.waypoints.Keys[i].endWaypoint)
            {
                //if the end waypoint is found, force that to be the next waypoint.
                DecideNextWaypoint(new KeyValuePair<PathWaypoint, bool>(currentWaypoint.waypoints.Keys[i], currentWaypoint.waypoints.Values[i]), new KeyValuePair<PathWaypoint, bool>(currentWaypoint.waypoints.Keys[i], currentWaypoint.waypoints.Values[i]));
                return true;
            }
        }
        return false;
    }
}
