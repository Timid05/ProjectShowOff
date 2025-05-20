using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] PathWaypoint startWaypoint;
    //GameObject currentWaypoint;
    PathWaypoint nextWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        //currentWaypoint = startWaypoint;
        DecidePathDirection(startWaypoint);
    }

    void DecidePathDirection(PathWaypoint currentWaypoint)
    {
        // If the current waypoint isn't the end one, then we decide what the next waypoint should be.
        if(!currentWaypoint.EndWaypoint)
        {

        }
    }
}
