using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PlayerActions
{
    public static Action OnPlayerHit;
    public static Action<int> OnPlayerDamaged;
    public static Action<int, int> OnHealthUpdated;
    public static Action OnPlayerDead;
}
