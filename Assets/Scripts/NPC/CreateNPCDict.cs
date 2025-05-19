using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateNPCDict : MonoBehaviour
{
    Dictionary<string, NPCInteraction> _NPCs = new Dictionary<string, NPCInteraction>();

    // Give Dictionary to game manager.
    public static event Action<Dictionary<string, NPCInteraction>> OnDictCreated;

    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag == "NPC")
            {
                Debug.Log(child.name);
                _NPCs.Add(child.name, child.GetComponent<NPCInteraction>());
            }
        }
        if(OnDictCreated != null) { OnDictCreated(_NPCs); }
    }
}
