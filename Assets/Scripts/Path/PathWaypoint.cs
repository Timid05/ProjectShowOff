using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWaypoint : MonoBehaviour
{
    [field: SerializeField] public bool endWaypoint { get; private set; } = false;

    [field: SerializeField] public UDictionary<PathWaypoint, bool> waypoints { get; private set; }

    [field: SerializeField] public GameObject northPath { get; private set; }
    [field: SerializeField] public GameObject southPath { get; private set; }
    [field: SerializeField] public GameObject westPath { get; private set; }
    [field: SerializeField] public GameObject eastPath { get; private set; }

    private void Start()
    {
        // Disable all paths at the start. They get reactivated during path generation.
        if(northPath != null) { northPath.SetActive(false); }
        if(southPath != null) { southPath.SetActive(false); }
        if(westPath != null) { westPath.SetActive(false); }
        if(westPath != null) { eastPath.SetActive(false); }
    }
}
