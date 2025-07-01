using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapVisibility : MonoBehaviour
{
    [SerializeField] KeyCode mapButton;
    [SerializeField]
    float openingTimer = 15f;
    bool mapAvailable = true;
    public static event Action<bool> OnMapButtonPressed;
    bool mapOpen = false;

    private void Awake()
    {
        PlayerInteraction.OnCharacterTalk += MapAvailable;       
    }

    private void Start()
    {
        EnemiesInfo.DisableEnemies();
        StartCoroutine(MapOpenTimer());
    }

    private void OnDisable()
    {
        PlayerInteraction.OnCharacterTalk -= MapAvailable;
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
        
        if (!GameStateActions.mapOpened && newVisibility == false && mapOpen)
        {
            GameStateActions.OnFirstMapOpen?.Invoke();
            GameStateActions.mapOpened = true;
            EnemiesInfo.EnableEnemies();
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(newVisibility);
        }
        mapOpen = newVisibility;

        // Prevents characters from being talked to when the flashlight is active.
        if (OnMapButtonPressed != null) { OnMapButtonPressed(newVisibility); }
    }

    void MapAvailable(bool characterTalking)
    {
        // Makes map unavailable when talking to characters and vice versa.
        mapAvailable = !characterTalking;
        if(characterTalking) { ChangeChildrenVisibility(false); }
    }

    IEnumerator MapOpenTimer()
    {
        yield return new WaitForSeconds(openingTimer);
        GameStateActions.OnFirstMapOpen?.Invoke();
        GameStateActions.mapOpened = true;
        EnemiesInfo.EnableEnemies();
    }
}
