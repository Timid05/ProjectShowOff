using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapAnimation : MonoBehaviour
{
    Animator anim;
    bool currentVisibility = false;
    [SerializeField] KeyCode mapButton;

    public static event Action<bool> OnMapVisibilityChanged;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        //MapVisibility.OnMapButtonPressed += MapVisibilityChange;
    }

    private void Update()
    {
        // Map is visible if the player holds down the map button
        if (Input.GetKeyDown(mapButton)) { MapVisibilityChange(true); }
        if (Input.GetKeyUp(mapButton)) { MapVisibilityChange(false); }

        // Send out delegate once we've reached the idle stage or returning to start after clsoing the map, if the delegate hasn't been send yet.
        if(anim != null && ((anim.GetCurrentAnimatorStateInfo(0).IsName("Map Idle") && !currentVisibility) || (anim.GetCurrentAnimatorStateInfo(0).IsName("Start") && currentVisibility)) && transform.GetChild(0).gameObject.activeSelf != currentVisibility) 
        {
            currentVisibility = transform.GetChild(0).gameObject.activeSelf;
            //Debug.LogFormat(" Changing map status when: Visibility {0}", currentVisibility);
            if (OnMapVisibilityChanged != null) { OnMapVisibilityChanged(currentVisibility); }

        }
    }

    void MapVisibilityChange(bool mapVisibility)
    {
        
        if (anim != null)
        {

            if(mapVisibility) 
            {
                //Debug.Log("Opening map.");
                anim.SetTrigger("mapOpen"); 
            }
            else 
            {
                //Debug.Log("Closing map.");
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
        //MapVisibility.OnMapButtonPressed += MapVisibilityChange;
    }
}
