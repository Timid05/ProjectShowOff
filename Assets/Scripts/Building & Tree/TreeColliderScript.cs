using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeColliderScript : MonoBehaviour
{
    TreeInstance tree;

    public void SetTree(TreeInstance pTree)
    {
        tree = pTree;
    }

    public TreeInstance GetTree()
    {
        return tree;
    }
}
