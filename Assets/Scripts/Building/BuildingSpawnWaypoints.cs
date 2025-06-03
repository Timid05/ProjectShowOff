using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingSpawnWaypoints : MonoBehaviour
{
    [SerializeField] List<Vector3> spawnWaypoints;
    public static event Action<List<Vector3>, CapsuleCollider> OnShareWaypoints;

    public List<Vector3> GetWaypoints()
    {
        return spawnWaypoints;
    }

    private void Start()
    {
        // This will be used to identify the area that the waypoints belong to.
        CapsuleCollider area = gameObject.GetComponent<CapsuleCollider>();
        if(OnShareWaypoints != null) { OnShareWaypoints(spawnWaypoints, area); }
    }
}