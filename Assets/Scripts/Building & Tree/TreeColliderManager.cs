using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeColliderManager : MonoBehaviour
{
    TerrainData terrainData;
    [SerializeField] GameObject treeColliderObj;

    // We can't change the tree instances list directly so we have to create a duplicate list and update that one.
    List<TreeInstance> updatedTreeInstances;
    // Tree instances defy the laws of unity and changes to them are permanent and don't reset after exiting runtime for some baffling reason. So we save the tree instances here and set them to this aonce the application is exited.
    TreeInstance[] startTreeInstances;

    private void Awake()
    {
        BuildingRemoveTrees.OnTreeFound += RemoveTree;
        BuildingSpawner.OnTreesCleared += RemoveTreeColliders;
    }

    void Start()
    {
        terrainData = gameObject.GetComponent<Terrain>().terrainData;
        if(terrainData == null) { return; }
        startTreeInstances = terrainData.treeInstances;
        updatedTreeInstances = new List<TreeInstance>(terrainData.treeInstances);
        Debug.Log("Amount of tree instances before deletion: " + terrainData.treeInstanceCount);

        // Add colliders to all trees.
        for (int i = 0; i < terrainData.treeInstanceCount; i++)
        {
            TreeInstance tree = terrainData.treeInstances[i];
            //Check if the treeInstance is actually a tree and not a bush or grass.
            if(terrainData.treePrototypes[tree.prototypeIndex].prefab.CompareTag("Tree"))
            {
                Vector3 worldTreePos = Vector3.Scale(tree.position, terrainData.size) + Terrain.activeTerrain.transform.position;
                //Debug.Log("Placing tree collider at " + worldTreePos);
                GameObject placedTreeCollider = Instantiate(treeColliderObj, worldTreePos, treeColliderObj.transform.rotation, gameObject.transform);

                CapsuleCollider capsuleCollider = placedTreeCollider.GetComponent<CapsuleCollider>();
                if (capsuleCollider != null) { capsuleCollider.transform.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale); }

                // We give the index so that we can remove the tree if the collider has been detected by the removal collider. 
                TreeColliderScript treeColliderScript = placedTreeCollider.GetComponent<TreeColliderScript>();
                if (treeColliderScript != null)
                {
                    treeColliderScript.SetTree(tree);
                }
            }
        }
    }

    void RemoveTreeColliders()
    {
        Debug.Log("Amount of tree instances after deletion: " + terrainData.treeInstanceCount);
        RefreshTerrain();
        // After the trees near buildings have been removed we can delete all the tree colliders that were spawned.
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    
    void RemoveTree(TreeInstance tree)
    {
        if(terrainData != null) 
        {
            //Debug.Log("Tree being deleted.");
            updatedTreeInstances.Remove(tree);
            terrainData.treeInstances = updatedTreeInstances.ToArray();
            //RefreshTerrain();
        }
    }

    void RefreshTerrain()
    {
        // We refresh the terrain to clear the leftover colliders of the removed trees.
        gameObject.GetComponent<TerrainCollider>().enabled = false;
        //Debug.Log("Brief pause in the run line.");
        gameObject.GetComponent<TerrainCollider>().enabled = true;
    }

    void OnApplicationQuit()
    {
        // restore original trees
        if(startTreeInstances != null)
        {
            Debug.Log("Reset tree count to: " + startTreeInstances.Length);
            terrainData.treeInstances = startTreeInstances;
        }
    }

    private void OnDestroy()
    {
        BuildingRemoveTrees.OnTreeFound -= RemoveTree;
        BuildingSpawner.OnTreesCleared -= RemoveTreeColliders;
    }
}
