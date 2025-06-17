using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRemoveTrees : MonoBehaviour
{
    SphereCollider removalArea;

    private void Awake()
    {
        BuildingSpawner.OnBuildingSpawned += MoveRemoverArea;
    }

    private void Start()
    {
        removalArea = gameObject.GetComponent<SphereCollider>();
    }

    void MoveRemoverArea(Vector3 buildingPosition)
    {
        Debug.Log("Tree remover moved to position " + buildingPosition);
        if(removalArea != null)
        {
            removalArea.transform.position = buildingPosition;
            if(!removalArea.enabled) { removalArea.enabled = true; }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogFormat("Object {0} with tag {1} in removal range.", other.gameObject.name, other.gameObject.tag);
        if(other.gameObject.CompareTag("Tree"))
        {
            Debug.Log("Deleting tree at position " + other.transform.position);
        }
    }

    private void OnDestroy()
    {
        BuildingSpawner.OnBuildingSpawned -= MoveRemoverArea;
    }
}
