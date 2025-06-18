using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeColliderScript : MonoBehaviour
{
    int treeIndex = -1;

    public int GetTreeIndex(int setIndex = 0)
    {
        // The first time the function is called it can be used to set the tree index.
        if(treeIndex < 0) { treeIndex = setIndex; }
        return treeIndex;
    }
}
