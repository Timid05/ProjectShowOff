using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingRemoveTrees : MonoBehaviour
{
    SphereCollider removalArea;
    bool treesCleared = false;
    public static event Action<TreeInstance> OnTreeFound;

    private void Start()
    {
        removalArea = gameObject.GetComponent<SphereCollider>();
    }

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
        if(other.gameObject.CompareTag("Tree"))
        {
            TreeColliderScript treeColliderScript = other.gameObject.GetComponent<TreeColliderScript>();
            if(OnTreeFound != null && treeColliderScript != null) { OnTreeFound(treeColliderScript.GetTree()); }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Tree") && treesCleared) { treesCleared = false; }
    }
}
