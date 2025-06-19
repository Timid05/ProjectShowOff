using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapVisibility : MonoBehaviour
{
    [SerializeField] KeyCode mapButton;
    bool mapAvailable = true;
    public static event Action<bool> OnMapButtonPressed;

    private void Awake()
    {
        PlayerInteraction.OnCharacterTalk += MapAvailable;
    }

    void Update()
    {
        if(mapAvailable && transform.childCount != 0)
        {
            // Map is visible if the player holds down the map button
            if (Input.GetKeyDown(mapButton)) { ChangeChildrenVisibility(true); }
            if (Input.GetKeyUp(mapButton)) { ChangeChildrenVisibility(false); }
        }
    }


    void ChangeChildrenVisibility(bool newVisibility)
    {
        if (!GameStateActions.mapOpened)
        {
            GameStateActions.OnFirstMapOpen?.Invoke();
            GameStateActions.mapOpened = true;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(newVisibility);
        }
        
        // Prevents characters from being talked to when the flashlight is active.
        if(OnMapButtonPressed != null) { OnMapButtonPressed(newVisibility); }
    }

    void MapAvailable(bool characterTalking)
    {
        // Makes map unavailable when talking to characters and vice versa.
        mapAvailable = !characterTalking;
        if(characterTalking) { ChangeChildrenVisibility(false); }
    }
}
