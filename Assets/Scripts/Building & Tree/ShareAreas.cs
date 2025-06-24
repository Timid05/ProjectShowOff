using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShareAreas : MonoBehaviour
{

    public static event Action<List<CapsuleCollider>> OnAreaShare;
    List<CapsuleCollider> areas;

    void Start()
    {
        areas = new List<CapsuleCollider>();
        for(int i = 0; i < transform.childCount; i++)
        {
            CapsuleCollider area = transform.GetChild(i).GetComponent<CapsuleCollider>();
            if (area != null) { areas.Add(area); }
        }
        if(OnAreaShare != null) { OnAreaShare(areas); }
    }
}
