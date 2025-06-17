using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeColliderManager : MonoBehaviour
{
    TerrainData terrainData;
    [SerializeField] GameObject treeColliderObj;

    void Start()
    {
        terrainData = gameObject.GetComponent<Terrain>().terrainData;
        if(terrainData == null) { return; }

        // Add colliders to all trees.
        foreach(TreeInstance tree in terrainData.treeInstances)
        {
            Vector3 worldTreePos = Vector3.Scale(tree.position, terrainData.size) + Terrain.activeTerrain.transform.position;
            //Debug.Log("Placing tree collider at " + worldTreePos);
            GameObject placedTreeCollider = Instantiate(treeColliderObj, worldTreePos, treeColliderObj.transform.rotation, gameObject.transform);
            CapsuleCollider capsuleCollider = placedTreeCollider.GetComponent<CapsuleCollider>();
            if(capsuleCollider != null) { capsuleCollider.transform.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale); }
        }
    }

}
