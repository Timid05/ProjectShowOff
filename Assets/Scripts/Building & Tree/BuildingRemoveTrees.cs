using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingRemoveTrees : MonoBehaviour
{
    bool treesCleared = false;
    public static event Action<TreeInstance> OnTreeFound;

    private void Start()
    {

    }

    private void Update()
    {
        if(!treesCleared) { treesCleared = true; }
    }

    public bool GetTreeStatus()
    {
        return treesCleared;
    }

    public void SetRemovalAreaSize(float size)
    {
        SphereCollider removalArea = gameObject.GetComponent<SphereCollider>();
        if (removalArea != null)
        { 
            removalArea.radius = size;
            //Debug.LogFormat("Removal area radius {0} should be set to: {1}", removalArea.radius, size);
        }
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
