using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingSpawner : MonoBehaviour
{
    [field: SerializeField] public UDictionary<CapsuleCollider, GameObject> spawnableBuildings { get; private set; }
    [SerializeField] GameObject removerObject;
    List<Vector3> buildingWaypoints;

    Dictionary<GameObject, List<GameObject>> areaPathObjects;
    int emptyAreas = 0;

    List<GameObject> removerAreas;
    bool treesCleared = false;

    public static event Action<Vector3> OnBuildingSpawned;
    public static event Action OnTreesCleared;

    private void Awake()
    {
        ShareMainPath.OnMainPathOnly += GetAreaPathObjects;
    }

    private void Start()
    {
        areaPathObjects = new Dictionary<GameObject, List<GameObject>>();
        removerAreas = new List<GameObject>();
    }

    private void Update()
    {
        // Once all area paths are added we can start placing the buildings.
        if(areaPathObjects.Count == spawnableBuildings.Count)
        {
            // Iterating through the areas like this instead of using delegates allows the trees to be cleared from each building.
            foreach(KeyValuePair<GameObject, List<GameObject>> areaPathObject in areaPathObjects)
            {
                if(areaPathObject.Value.Count != 0) { PlaceObject(areaPathObject.Value, areaPathObject.Key); }
                // Allows trees to be cleared even if one of the areas has no main path objects.
                else { emptyAreas++; }
            }

            // Clear the list so, this only happens once.
            areaPathObjects.Clear();
        }

        // We check if all areas have cleared their trees or not
        if(!treesCleared && removerAreas.Count == spawnableBuildings.Count - emptyAreas)
        {
            int clearCount = 0;
            foreach(GameObject removerArea in removerAreas)
            {
                BuildingRemoveTrees brt = removerArea.GetComponent<BuildingRemoveTrees>();
                if(brt != null && brt.GetTreeStatus()) { clearCount++; }
            }

            // If all areas return true, that means all areas have had their trees removed.
            if(clearCount == removerAreas.Count && OnTreesCleared != null) 
            {
                Debug.Log("All areas have cleard their trees!");
                treesCleared = true;
                OnTreesCleared(); 
            }
        }
    }

    void GetAreaPathObjects(List<GameObject> mainPathObjects, GameObject areaObject)
    {
        areaPathObjects.Add(areaObject, mainPathObjects);
    }


    // Place the object so that it is touching a road piece.
    void PlaceObject(List<GameObject> mainPathObjects, GameObject areaObject)
    {
        CapsuleCollider area = areaObject.GetComponent<CapsuleCollider>();
        BuildingSpawnWaypoints buildingSpawnWaypoints = areaObject.GetComponent<BuildingSpawnWaypoints>();

        if (spawnableBuildings == null || area == null || !spawnableBuildings.ContainsKey(area)) { return; }

        spawnableBuildings.TryGetValue(area, out GameObject spawnObject);

        //Pick a random object from the main path to place to object next to.
        GameObject spawnPath = mainPathObjects[UnityEngine.Random.Range(0, mainPathObjects.Count)];
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

                // Makes the spawned object face the path it was spawned next to. The rotation mask makes sure we only rotate on the Y-axis.
                Vector3 rotationMask = new Vector3(0, 1, 0);
                Vector3 lookRotation = Quaternion.LookRotation(spawnPath.transform.position - closestWaypoint).eulerAngles;
                Quaternion spawnRotation = Quaternion.Euler(Vector3.Scale(lookRotation, rotationMask));

                //Debug.LogFormat("Spawning at waypoint {0} with rotation {1}", closestWaypoint, spawnRotation);
                Instantiate(spawnObject, closestWaypoint, spawnRotation, gameObject.transform);

                SpawnRemoverArea(closestWaypoint);
                return;
            }
        } 
        // If spawn waypoints arent' set, Spawn the object at the center of the path as a failsafe.
        Instantiate(spawnObject, spawnPath.transform.position, spawnObject.transform.rotation, gameObject.transform);
        SpawnRemoverArea(spawnPath.transform.position);
    }

    void SpawnRemoverArea(Vector3 spawnPosition)
    {
        // Spawn object so the trees from around the spawned building can be removed.
        if (removerObject != null)
        {
            Debug.Log("Spawning tree remover area");
            GameObject removerArea = Instantiate(removerObject, spawnPosition, removerObject.transform.rotation, gameObject.transform);
            removerAreas.Add(removerArea);
        }
    }

    private void OnDestroy()
    {
        ShareMainPath.OnMainPathOnly -= GetAreaPathObjects;
    }
}
