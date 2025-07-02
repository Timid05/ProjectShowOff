using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapAnimation : MonoBehaviour
{
    Animator anim;
    bool currentVisibility = false;
    [SerializeField] KeyCode mapButton;
    bool mapAvailable = true;
    bool mapOpened = false;

    public static event Action<bool> OnMapVisibilityChanged;
    public static event Action<bool> OnMapButtonPressed;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        PlayerInteraction.OnCharacterTalk += MapAvailable;
        //MapVisibility.OnMapButtonPressed += MapVisibilityChange;
    }

    private void Update()
    {
        // Map is visible if the player holds down the map button
        if (Input.GetKeyDown(mapButton)) { MapVisibilityChange(true); }
        if (Input.GetKeyUp(mapButton)) { MapVisibilityChange(false); }

        // Send out delegate once we've reached the idle stage or returning to start after clsoing the map, if the delegate hasn't been send yet.
        if(anim != null && mapAvailable && ((anim.GetCurrentAnimatorStateInfo(0).IsName("Map Idle") && !currentVisibility) || (anim.GetCurrentAnimatorStateInfo(0).IsName("Start") && currentVisibility)) && transform.GetChild(0).gameObject.activeSelf != currentVisibility) 
        {
            currentVisibility = transform.GetChild(0).gameObject.activeSelf;
            //Debug.LogFormat(" Changing map status when: Visibility {0}", currentVisibility);
            if (OnMapVisibilityChanged != null) 
            { 
                OnMapVisibilityChanged(currentVisibility);

                // We hide the physical map once the UI is up to make the animation match better without being able to see the map from behind the UI.
                if (currentVisibility) { SwitchPhysicalMapVisibility(!currentVisibility); }
            }
        }
    }

    void MapVisibilityChange(bool mapVisibility)
    {
        
        if (anim != null && mapAvailable)
        {
            // Prevents characters from being talked to when the flashlight is active.
            if (OnMapButtonPressed != null) { OnMapButtonPressed(mapVisibility); }

            if (mapVisibility) 
            {
                //Debug.Log("Opening map.");
                anim.SetTrigger("mapOpen");
                mapOpened = true;
            }
            // This prevents a bug where it tries to close the map when it was never opened. This can happen if the first press is negated by talking to Tanfana.
            else if(mapOpened)
            {
                //Debug.Log("Closing map.");
                anim.SetTrigger("mapClose");
                mapOpened = false;
            }

            SwitchPhysicalMapVisibility(mapVisibility);
        }
    }

    void SwitchPhysicalMapVisibility(bool mapVisibility)
    {
        // Enable/Disable map visuals
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(mapVisibility);
        }
    }

    void MapAvailable(bool characterTalking)
    {
        // Makes map unavailable when talking to characters and vice versa.
        mapAvailable = !characterTalking;
        //if (characterTalking) { SwitchPhysicalMapVisibility(false); }
    }

    private void OnDestroy()
    {
        PlayerInteraction.OnCharacterTalk -= MapAvailable;
        //MapVisibility.OnMapButtonPressed += MapVisibilityChange;
    }
}
