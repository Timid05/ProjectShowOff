using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathCategorizeWaypoint : MonoBehaviour
{
    public static event Action<List<PathWaypoint>, CapsuleCollider> OnEntryWaypointsCategorized;
    List<PathWaypoint> waypointsInArea;
    List<PathWaypoint> entryWaypointsInArea;
    bool waypointsSent = false;
    CapsuleCollider area;

    private void Start()
    {
        waypointsInArea = new List<PathWaypoint>();
        entryWaypointsInArea = new List<PathWaypoint>();
        area = gameObject.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        //Convert the list of waypoints in the area to only the ones accessible from outside of that area.
        if (waypointsInArea.Count != 0 && entryWaypointsInArea.Count == 0 && !waypointsSent) 
        { 
            EntryWaypoint(); 
        }

        //If the list of waypoints hasn't been sent yet, it can be sent.
        if (area != null && OnEntryWaypointsCategorized != null && entryWaypointsInArea.Count != 0 && !waypointsSent)
        {
            waypointsSent = true;
            OnEntryWaypointsCategorized(entryWaypointsInArea, area);
            //Debug.LogFormat("Area {0} sent waypoints with size {1}", gameObject.name, entryWaypointsInArea.Count);
        }
    }

    void EntryWaypoint()
    {
        foreach(PathWaypoint waypoint in  waypointsInArea)
        {
            for (int i = 0; i < waypoint.waypoints.Count; i++)
            {
                // If the waypoint is not within the current area and that waypoint can reach the current waypoint, then this waypoint can be considered an entry one.
                if (waypoint.waypoints.Keys[i] != null && !waypointsInArea.Contains(waypoint.waypoints.Keys[i]) && Reachable(waypoint.waypoints.Keys[i], waypoint))
                {
                    //Debug.LogFormat("Waypoint {0} can be used to enter area {1}", waypoint.name, gameObject.name);
                    entryWaypointsInArea.Add(waypoint);
                    break;
                }
            }
        }
    }

    bool Reachable(PathWaypoint outsideWaypoint, PathWaypoint waypointToReach)
    {
        for (int i = 0; i < outsideWaypoint.waypoints.Count; i++)
        {
            if (outsideWaypoint.waypoints.Keys[i] != null && outsideWaypoint.waypoints.Keys[i] == waypointToReach)
            {
                if(outsideWaypoint.waypoints.Values[i]) { return true; }
                else { return false; }
            }
        }
        return false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Waypoint"))
        {
            //Debug.LogFormat("Area {0} has waypoint {1}", gameObject.name, other.gameObject.name);
            PathWaypoint waypoint = other.gameObject.GetComponent<PathWaypoint>();

            // Only add a waypoint if it can be reached from outside of that area.
            if(waypoint != null) { waypointsInArea.Add(waypoint); }
        }
    }
}
