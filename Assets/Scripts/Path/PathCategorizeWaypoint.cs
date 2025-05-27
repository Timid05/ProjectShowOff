using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathCategorizeWaypoint : MonoBehaviour
{
    public static event Action<List<PathWaypoint>, CapsuleCollider> OnWaypointsCategorized;
    List<PathWaypoint> waypointsInArea;
    bool waypointsSent = false;
    CapsuleCollider area;

    private void Start()
    {
        waypointsInArea = new List<PathWaypoint>();
        area = gameObject.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        // If the list of waypoints hasn't been sent yet, it can be sent.
        if(area != null && OnWaypointsCategorized != null && waypointsInArea.Count != 0 && !waypointsSent)
        {
            waypointsSent = true;
            OnWaypointsCategorized(waypointsInArea, area);
            Debug.LogFormat("Area {0} sent waypoints with size {1}", gameObject.name, waypointsInArea.Count);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Waypoint"))
        {
            //Debug.LogFormat("Area {0} has waypoint {1}", gameObject.name, other.gameObject.name);
            PathWaypoint waypoint = other.gameObject.GetComponent<PathWaypoint>();

            if(waypoint != null) { waypointsInArea.Add(waypoint); }
        }
    }
}
