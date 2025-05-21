using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]

public class FollowPath : MonoBehaviour
{
    public enum FollowType { BackAndForth, Cycle, Target}
    public FollowType followType;
    public Transform target;
    [HideInInspector]
    public NavMeshAgent navmeshAgent;
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
        navmeshAgent = GetComponent<NavMeshAgent>();  

        if (path.Count < 2)
        {
            Debug.LogError("Too little waypoints in path");
        }

        if (navmeshAgent != null)
        {
            navmeshAgent.enabled = true;
            navmeshAgent.SetDestination(path[0]);
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
                navmeshAgent.SetDestination(path[toWaypoint]);
                return;
            }

            if (toWaypoint == path.Count - 1)
            {
                toWaypoint--;
                movingBack = true;
                navmeshAgent.SetDestination(path[toWaypoint]);
                return;
            }

            if (movingBack)
            {
                toWaypoint--;
                navmeshAgent.SetDestination(path[toWaypoint]);
                return;
            }
            else
            {
                toWaypoint++;
                navmeshAgent.SetDestination(path[toWaypoint]);
                return;
            }

        }

        toWaypoint++;

        if (toWaypoint >= path.Count && followType == FollowType.Cycle)
        {
            toWaypoint = 0;
            navmeshAgent.SetDestination(path[toWaypoint]);
        }
    }

    public List<Vector3> GetPath()
    {
        return path;
    }

    public void ClearPath()
    {
        path.Clear();
    }

    public void RemoveNearestWaypoint()
    {
        path.Remove(path[FindNearestWaypoint()]);
    }

    bool AtTarget()
    {
        if ((navmeshAgent.destination - transform.position).sqrMagnitude < targetOffset && followType == FollowType.Target)
        {
            Debug.Log("Reached target");
            return true;
        }
        else return false;
    }

    bool AtDestination()
    {  
        if ((navmeshAgent.destination - transform.position).sqrMagnitude < waypointOffset)
        {
            Debug.Log("Destination reached");
            return true;
        }
        else return false;
    }

    public void AddWaypoint(Vector3 point)
    {
        path.Add(point);
    }

    Vector3 direction;
    void Move()
    {
        if (AtTarget())
        {
            Debug.Log("slay");
        }

        if (followType == FollowType.Target)
        {
            if (navmeshAgent.destination != target.position)
            {
                navmeshAgent.SetDestination(target.position);
            }
            return;
        }

        if (AtDestination())
        {
            NextWaypoint();
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
                navmeshAgent.SetDestination(path[toWaypoint]);
            }

            oldType = followType;
        }
    }
}
