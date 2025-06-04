using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HellhoundActions 
{
    public static Action OnHellhoundFightTriggered;
    public static Action OnGrowlTriggered;
    public static Action OnCharge;
    public static Action OnPounce;
    public static Action<bool> OnHellhoundFlashable;
    public static Action OnHellhoundFlashed;
    public static Action OnHellhoundDeath;
}
