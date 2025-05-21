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

    // Start is called before the first frame update
    void Start()
    {
        pastDirections = new List<bool>();
        // This allows the path to start generating properly.
        prevWaypoint = startWaypoint.SouthWaypoint;
        currentWaypoint = startWaypoint;
        //GeneratePath(startWaypoint);
    }

    private void Update()
    {
        // We stop generating if the current waypoint is the end one or the path manages to double back on itself and reach a disabled waypoint.
        // Othwerwise  we decide what the next waypoint should be.
        if (!currentWaypoint.EndWaypoint && currentWaypoint.enabled) { GeneratePath(); }
        //else { Debug.Log("Generation ended at " + currentWaypoint); }
    }

    void GeneratePath()
    {
        Debug.Log("Generating for waypoint: " + currentWaypoint.name);
        currentWaypoint.GetComponent<MeshRenderer>().enabled = true;

        // Having randomisation be a seperate function allows for a more concrete variable in this function.
        generateRight = RandomisePathDirection();
        Debug.Log("Going right? " + generateRight);

        //Determine which waypoints are left and right based on which direction the path is coming from. If it is unknown, default to west being left and east being right.
        if (prevWaypoint != null)
        {
            List<PathWaypoint> waypoints = new List<PathWaypoint> { currentWaypoint.NorthWaypoint, currentWaypoint.SouthWaypoint, currentWaypoint.WestWaypoint, currentWaypoint.EastWaypoint };
            // This will have the waypoints that the next one will be chosen from.
            List<PathWaypoint> possibleNextWaypoints = new List<PathWaypoint>();
            //Debug.Log("Waypoint count: " + waypoints.Count);
            for (int i = 0; i < waypoints.Count; i++)
            {
                if (prevWaypoint == waypoints[i])
                {
                    waypoints.RemoveAt(i);
                    // Add the waypoints perpendicular to the direction the path is coming from.
                    if (prevWaypoint == currentWaypoint.NorthWaypoint || prevWaypoint == currentWaypoint.SouthWaypoint)
                    {
                        possibleNextWaypoints.Add(currentWaypoint.WestWaypoint);
                        possibleNextWaypoints.Add(currentWaypoint.EastWaypoint);
                    }
                    else
                    {
                        possibleNextWaypoints.Add(currentWaypoint.NorthWaypoint);
                        possibleNextWaypoints.Add(currentWaypoint.SouthWaypoint);
                    }

                    //Remove possible waypoints from the original list. This will allow the option to add a backup option if necessary.
                    waypoints.Remove(possibleNextWaypoints[0]);
                    waypoints.Remove(possibleNextWaypoints[1]);
                    break;
                }
            }
            //Check if one of the possible waypoints is missing.
            for (int i = 0; i < possibleNextWaypoints.Count; i++)
            {
                if (possibleNextWaypoints[i] == null)
                {
                    //Debug.LogFormat("Replacing missing waypoint at index {0} with {1}", i, waypoints[0]);
                    //If it is missing, replace it with the one remaining waypoint in the original list.
                    possibleNextWaypoints[i] = waypoints[0];
                    break;
                }
            }


            //Swap the results of left and right if the waypoints orientation is North of East, so it matches their viewpoint.
            bool swapResults = false;
            if (prevWaypoint == currentWaypoint.NorthWaypoint || prevWaypoint == currentWaypoint.EastWaypoint) { swapResults = true; }

            if (swapResults) { DecideNextWaypoint(possibleNextWaypoints[1], possibleNextWaypoints[0]); }
            else { DecideNextWaypoint(possibleNextWaypoints[0], possibleNextWaypoints[1]); }
        }
        else { DecideNextWaypoint(currentWaypoint.WestWaypoint, currentWaypoint.EastWaypoint); }
    }

    void DecideNextWaypoint(PathWaypoint leftWaypoint, PathWaypoint rightWaypoint)
    {
        Debug.LogFormat("Choosing next waypoint between {0} and {1}.", leftWaypoint.name, rightWaypoint.name);

        // If one of the options has already been visited, the direction needs to be forced the other way.
        if(!leftWaypoint.enabled)
        {
            Debug.Log("Forcing direction right");
            generateRight = true;
        }
        else if(!rightWaypoint.enabled)
        {
            Debug.Log("Forcing direction left");
            generateRight = false;
        }
        // Add the direction to the list after any potential forcing, so the accurate version gets added.
        pastDirections.Add(generateRight);

        prevWaypoint = currentWaypoint;
        if (generateRight) { currentWaypoint = rightWaypoint; }
        else { currentWaypoint = leftWaypoint; }
        // Disable previous waypoint to note that it's already been visited.
        prevWaypoint.enabled = false;
    }

    bool RandomisePathDirection()
    {
        // Restrict direction if the past two directions have been the same.
        if (pastDirections.Count >= 2 && pastDirections[pastDirections.Count - 1] == pastDirections[pastDirections.Count - 2])
        {
            Debug.Log("Forcing direction.");
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
}
