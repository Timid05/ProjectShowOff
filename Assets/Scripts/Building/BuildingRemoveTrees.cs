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
        if(other.gameObject.GetType() == typeof(TreeInstance))
        {
            Debug.Log("Deleting tree at position " + other.transform.position);
        }
    }

    private void OnDestroy()
    {
        BuildingSpawner.OnBuildingSpawned -= MoveRemoverArea;
    }
}
