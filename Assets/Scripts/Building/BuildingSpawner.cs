using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [field: SerializeField] public UDictionary<CapsuleCollider, GameObject> spawnableBuildings { get; private set; }
    List<Vector3> buildingWaypoints;
    CapsuleCollider waypointArea;

    private void Awake()
    {
        ShareMainPath.OnMainPathOnly += PlaceObject;
    }


    // Place the object so that it is touching a road piece.
    void PlaceObject(List<GameObject> mainPathObjects, GameObject areaObject)
    {
        CapsuleCollider area = areaObject.GetComponent<CapsuleCollider>();
        BuildingSpawnWaypoints buildingSpawnWaypoints = areaObject.GetComponent<BuildingSpawnWaypoints>();

        if (spawnableBuildings == null || area == null || !spawnableBuildings.ContainsKey(area)) { return; }

        spawnableBuildings.TryGetValue(area, out GameObject spawnObject);

        //Pick a random object from the main path to place to object next to.
        GameObject spawnPath = mainPathObjects[Random.Range(0, mainPathObjects.Count)];
        Debug.LogFormat("Placing object {0} in area {1} nearby waypoint {2}", spawnObject.name, area.name, spawnPath.name);

        if(buildingSpawnWaypoints != null)
        {
            // Get the spawn waypoints for the current area
            buildingWaypoints = buildingSpawnWaypoints.GetWaypoints();

            // Find the waypoint that is closest to the chosen object.
            if (buildingWaypoints != null && buildingWaypoints.Count != 0)
            {
                Vector3 closestWaypoint = transform.position;
                float closestDistance = -1;

                foreach (Vector3 waypoint in buildingWaypoints)
                {
                    float distance = Vector3.Distance(spawnPath.transform.position, waypoint);

                    // Update the closest waypoint if the current waypoint is closer.
                    if (closestDistance < 0 || distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestWaypoint = waypoint;
                    }
                }

                // Makes the spawned object face the path it was spawned next to.
                Quaternion spawnRotation = Quaternion.LookRotation((spawnPath.transform.position - closestWaypoint).normalized);

                Debug.LogFormat("Spawning at waypoint {0} with rotation {1}", closestWaypoint, spawnRotation);
                Instantiate(spawnObject, closestWaypoint, spawnRotation, gameObject.transform);
                return;
            }
        } 
        // If spawn waypoints arent' set, Spawn the object at the center of the path as a failsafe.
        Instantiate(spawnObject, spawnPath.transform.position, spawnObject.transform.rotation, gameObject.transform);
    }

    void OnGetWaypoints(List<Vector3> pBuildingWaypoints, CapsuleCollider pWaypointArea)
    {
        buildingWaypoints = pBuildingWaypoints;
        waypointArea = pWaypointArea;
    }

    private void OnDestroy()
    {
        ShareMainPath.OnMainPathOnly -= PlaceObject;
    }
}
