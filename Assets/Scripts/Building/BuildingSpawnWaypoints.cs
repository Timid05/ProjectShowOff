using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingSpawnWaypoints : MonoBehaviour
{
    [SerializeField] List<Vector3> spawnWaypoints;

    public List<Vector3> GetWaypoints()
    {
        return spawnWaypoints;
    }
}