using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    enum FollowType { BackAndForth, Cycle }
    [SerializeField] 
    FollowType followType;

    [SerializeField]
    public Vector3[] path;
    [HideInInspector]
    public float speed;
    [SerializeField]
    float offset;

    int toWaypoint = 0;

    bool movingBack = false;
    [HideInInspector]
    public bool drawPath = false;

    void Awake()
    {
        if (path.Length < 2)
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
            
            if (toWaypoint == path.Length - 1)
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

        if (toWaypoint >= path.Length && followType == FollowType.Cycle)
        {
            toWaypoint = 0;
        }

        Debug.Log("Moving to waypoint " + toWaypoint);
    }

    void Move()
    {
        Vector3 direction = path[toWaypoint] - transform.position;

        transform.position += (direction.normalized * speed) * Time.deltaTime;

        if (direction.sqrMagnitude < offset)
        {
            NextWaypoint();
        }
    }

    private void OnDrawGizmosSelected()
    {
        //For visualizing the path in the scene view
        if (drawPath)
        {
            for (int i = 0; i < path.Length; i++)
            {
                if (i != 0)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(path[i], path[i - 1]);
                }
            }
        }
    }


    void Update()
    {
        if (path.Length > 2)
        {
            Move();
        }
    }
}
