using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class GameStateActions
{
    //To notify when the start comic finishes
    public static Action OnComicFinish;
    //Logic to see if the map has been opened for the first time
    public static Action OnFirstMapOpen;
    public static bool mapOpened = false;
    //Notifying the first dome visit of the startPos in a run
    public static Action OnFirstDomeVisit;
    public static bool domeVisited = false;
    //Notifying when the startPos has picked up the chalice
    public static Action OnChaliceCollected;
    //Notifying when the startPos has returned the chalice to the statue
    public static Action OnChaliceReturned;
    //Action that tells listeners which choice the startPos made with tanfana, false being rejection and true being acceptance
    public static Action<bool> OnChoiceMade;
    public static bool acceptedChoice;
    //Action that notifies when the game gets pauses/unpaused
    public static Action<bool> OnGamePause;
    //Bool that keeps track of the players's dialogue state
    public static bool inDialogue = false;
    //Should fire when the player starts a NPC interaction
    public static Action<GameObject> OnNPCInteraction;
    //Should fire when an attack WW gets within range of the player
    public static Action OnFirstEnemyEncounter;
    public static bool firstEnemyEncountered = false;
    
    public static Action OnRespawnEnemies;

    public static bool playerDead = false;


    public static void Reset()
    {
        mapOpened = false;
        domeVisited = false;
        firstEnemyEncountered = false;
        playerDead = false;
        HellhoundActions.hellhoundFightOngoing = false;
    }
 }
