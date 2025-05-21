using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWaypoint : MonoBehaviour
{
    [field: SerializeField] public bool EndWaypoint { get; private set; } = false;
    [field: SerializeField] public PathWaypoint NorthWaypoint { get; private set; }
    [field: SerializeField] public PathWaypoint SouthWaypoint { get; private set; }
    [field: SerializeField] public PathWaypoint WestWaypoint { get; private set; }
    [field: SerializeField] public PathWaypoint EastWaypoint { get; private set; }
}
