using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [field: SerializeField] public UDictionary<CapsuleCollider, GameObject> spawnableBuildings { get; private set; }

    private void Awake()
    {
        PathGenerator.OnPathGenerated += PlaceObject;
    }


    // Place the object so that it is touching a road piece.
    void PlaceObject()
    {
        Debug.Log("Placing objects.");
        if(spawnableBuildings == null){ return; }
        foreach(KeyValuePair<CapsuleCollider, GameObject> pair in spawnableBuildings)
        {
            if(pair.Key == null || pair.Value == null) { continue; }
            Debug.LogFormat("Spawning object {0} in area {1}", pair.Value.name, pair.Key.name);
        }
    }

    // Adjust the placement of the object so that it's next to the road instead of inside of it.
    void AdjustObjectPlacement()
    {

    }

    // Rotate object to face the road it's closest to.
    void RotateObject()
    {

    }

    private void OnDestroy()
    {
        PathGenerator.OnPathGenerated -= PlaceObject;
    }
}
