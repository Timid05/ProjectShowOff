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
        BuildingSpawnWaypoints.OnShareWaypoints += OnGetWaypoints;
    }


    // Place the object so that it is touching a road piece.
    void PlaceObject(List<GameObject> mainPathObjects, CapsuleCollider area)
    {
        if (spawnableBuildings == null || !spawnableBuildings.ContainsKey(area)) { return; }

        spawnableBuildings.TryGetValue(area, out GameObject spawnObject);

        //Pick a random object from the main path to place to object next to.
        GameObject spawnPath = mainPathObjects[Random.Range(0, mainPathObjects.Count)];
        Debug.LogFormat("Placing object {0} in area {1} nearby waypoint {2}", spawnObject.name, area.name, spawnPath.name);

        // Find the waypoint that is closest to the chosen object.
        if (buildingWaypoints != null && area == waypointArea)
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

            Debug.Log("Spawning at waypoint " + closestWaypoint);
            Instantiate(spawnObject, closestWaypoint, spawnObject.transform.rotation, gameObject.transform);
        }
        else { Instantiate(spawnObject, spawnPath.transform.position, spawnObject.transform.rotation, gameObject.transform); }

        //Collider pathCol = spawnPath.GetComponent<Collider>();
        //if (pathCol != null)
        //{
        //    Instantiate(spawnObject, pathCol.ClosestPointOnBounds(area.center), spawnObject.transform.rotation, gameObject.transform);
        //}
        //else { Instantiate(spawnObject, spawnPath.transform.position, spawnObject.transform.rotation, gameObject.transform); }
        //Instantiate(spawnObject, spawnPath.transform.position, spawnObject.transform.rotation, gameObject.transform);
    }

    // Rotate object to face the road it's closest to.
    void RotateObject()
    {

    }

    void OnGetWaypoints(List<Vector3> pBuildingWaypoints, CapsuleCollider pWaypointArea)
    {
        buildingWaypoints = pBuildingWaypoints;
        waypointArea = pWaypointArea;
    }

    private void OnDestroy()
    {
        ShareMainPath.OnMainPathOnly -= PlaceObject;
        BuildingSpawnWaypoints.OnShareWaypoints -= OnGetWaypoints;
    }
}
