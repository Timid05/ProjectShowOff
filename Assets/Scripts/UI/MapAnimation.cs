using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimation : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        MapVisibility.OnMapButtonPressed += MapVisibilityChange;
    }

    void MapVisibilityChange(bool mapVisibility)
    {
        
        if (anim != null)
        {

            if(mapVisibility) 
            {
                Debug.Log("Opening map.");
                anim.SetTrigger("mapOpen"); 
            }
            else 
            {
                Debug.Log("Closing map.");
                anim.SetTrigger("mapClose"); 
            }

            // Enable/Disable map visuals
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(mapVisibility);
            }
        }
    }

    private void OnDestroy()
    {
        MapVisibility.OnMapButtonPressed += MapVisibilityChange;
    }
}
