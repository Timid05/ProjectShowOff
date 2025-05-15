using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public enum FollowType { BackAndForth, Cycle, Target }
    public FollowType followType;
    [SerializeField]
    private Transform target;
    [SerializeField]
    List<Vector3> path;
    [HideInInspector]
    public float speed;
    [SerializeField]
    float waypointOffset;
    [SerializeField]
    float targetOffset;

    int toWaypoint = 0;

    bool movingBack = false;
    [HideInInspector]
    public bool drawPath = false;

    void Awake()
    {
        if (path.Count < 2)
        {
            Debug.LogError("Too little waypoints in path");
        }
        else
        {
            transform.position = path[0];
        }
    }

    void NextWaypoint()
    {
        //Decides which waypoint we move to next depending on the chosen followtype

        if (followType == FollowType.BackAndForth)
        {
            if (toWaypoint == 0)
            {
                toWaypoint++;
                movingBack = false;
                return;
            }

            if (toWaypoint == path.Count - 1)
            {
                toWaypoint--;
                movingBack = true;
                return;
            }

            if (movingBack)
            {
                toWaypoint--;
                return;
            }
            else
            {
                toWaypoint++;
                return;
            }

        }

        toWaypoint++;

        if (toWaypoint >= path.Count && followType == FollowType.Cycle)
        {
            toWaypoint = 0;
        }

        Debug.Log("Moving to waypoint " + toWaypoint);
    }

    public List<Vector3> GetPath()
    {
        return path;
    }

    public void ClearPath()
    {
        path.Clear();
    }

    Vector3 direction;
    void Move()
    {
        if (followType == FollowType.Target)
        {
            direction = target.position - transform.position;

            if (direction.sqrMagnitude > targetOffset)
            {
                transform.position += (direction.normalized * speed) * Time.deltaTime;
            }
        }
        else
        {
            direction = path[toWaypoint] - transform.position;
            if (direction.sqrMagnitude < waypointOffset)
            {
                NextWaypoint();
            }
            transform.position += (direction.normalized * speed) * Time.deltaTime;
        }

        
    }

    private void OnDrawGizmosSelected()
    {
        //For visualizing the path in the scene view
        if (drawPath)
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (i != 0)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(path[i], path[i - 1]);
                }
            }
        }
    }

    private int FindNearestWaypoint()
    {
        int nearestWaypoint = 0;
        float shortestDistance = 0;

        for (int i = 0; i < path.Count; i++)
        {
            float distance = (path[i] - transform.position).sqrMagnitude;

            if (shortestDistance == 0 || distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestWaypoint = i;
            }
        }

        return nearestWaypoint;
    }



    FollowType oldType;
    void Update()
    {
        if (path.Count > 2)
        {
            Move();
        }

        if (oldType != followType)
        {
            if (oldType == FollowType.Target)
            {
                toWaypoint = FindNearestWaypoint();
            }

            oldType = followType;
        }
    }
}
