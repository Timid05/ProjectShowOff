using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWaypoint : MonoBehaviour
{
    [field: SerializeField] public bool EndWaypoint { get; private set; } = false;
    [field: SerializeField] public GameObject NorthWaypoint { get; private set; }
    [field: SerializeField] public GameObject SouthWaypoint { get; private set; }
    [field: SerializeField] public GameObject WestWaypoint { get; private set; }
    [field: SerializeField] public GameObject EastWaypoint { get; private set; }
}
