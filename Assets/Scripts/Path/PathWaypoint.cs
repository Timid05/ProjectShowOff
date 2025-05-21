using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWaypoint : MonoBehaviour
{
    [field: SerializeField] public bool EndWaypoint { get; private set; } = false;

    [field: SerializeField] public UDictionary<PathWaypoint, bool> waypoints { get; private set; }
}
