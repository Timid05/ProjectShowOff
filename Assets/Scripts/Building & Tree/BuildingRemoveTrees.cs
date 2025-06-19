using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingRemoveTrees : MonoBehaviour
{
    SphereCollider removalArea;
    bool treesCleared = false;
    public static event Action<TreeInstance> OnTreeFound;

    private void Awake()
    {
        //BuildingSpawner.OnBuildingSpawned += CreateRemoverArea;
    }

    private void Start()
    {
        removalArea = gameObject.GetComponent<SphereCollider>();
    }

    //void StartMoveRemoverArea(Vector3 buildingPosition) 
    //{
    //    StartCoroutine(MoveRemoverArea(buildingPosition));
    //}

    //IEnumerator MoveRemoverArea(Vector3 buildingPosition)
    //{
    //    Debug.Log("Moving remover area to position " + buildingPosition);
    //    if(removalArea != null)
    //    {
    //        removalArea.transform.position = buildingPosition;
    //        if(!removalArea.enabled) { removalArea.enabled = true; }
    //        yield return new WaitForFixedUpdate();
    //    }
    //}

    private void Update()
    {
        if(!treesCleared) { treesCleared = true; }
    }

    public bool GetTreeStatus()
    {
        return treesCleared;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogFormat("Object {0} with tag {1} in removal range.", other.gameObject.name, other.gameObject.tag);
        if(other.gameObject.CompareTag("Tree"))
        {
            //Debug.Log("Found tree at position " + other.transform.position);
            TreeColliderScript treeColliderScript = other.gameObject.GetComponent<TreeColliderScript>();
            if(OnTreeFound != null && treeColliderScript != null) { OnTreeFound(treeColliderScript.GetTree()); }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Tree") && treesCleared) { treesCleared = false; }
    }

    private void OnDestroy()
    {
        //BuildingSpawner.OnBuildingSpawned -= CreateRemoverArea;
    }
}
