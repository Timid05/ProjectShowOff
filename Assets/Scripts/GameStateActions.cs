using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateActions
{
    //To notify when the start comic finishes
    public static Action OnComicFinish;
    //Notifying the first dome visit of the player in a run
    public static Action OnFirstDomeVisit;
    //Notifying when the player has picked up the chalice
    public static Action OnChaliceCollected;
    //Notifying when the player has returned the chalice to the statue
    public static Action OnChaliceReturned;
    //Action that tells listeners which choice the player made with tanfana, false being rejection and true being acceptance
    public static Action<bool> OnChoiceMade;
}
