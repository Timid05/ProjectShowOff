using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemiesInfo
{
    static List<EnemyStateMachine> Enemies = new List<EnemyStateMachine>();
    public static Action<EnemyStateMachine.State> OnStateChange;
    public static Action OnEnemyAdded;
    public static Action OnEnemyRemoved;
    public static Action<GameObject> OnEnragedAttacks;

    public static bool HasAggressiveEnemies()
    {
        foreach (EnemyStateMachine m in Enemies)
        {
            if (m.currentState is AggressiveState)
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasEnragedEnemies()
    {
        foreach (EnemyStateMachine m in Enemies)
        {
            if (m.currentState is EnragedState)
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasDocileEnemies()
    {
        foreach (EnemyStateMachine m in Enemies)
        {
            if (m.currentState is DocileState)
            {
                return true;
            }
        }

        return false;
    }


    public static List<EnemyStateMachine> GetEnemyStateMachines()
    {
        return Enemies;
    }

    public static void AddStateMachine(EnemyStateMachine m)
    {
        Enemies.Add(m);
        OnEnemyAdded?.Invoke();
    }

    public static void RemoveStateMachine(EnemyStateMachine m)
    {
        Enemies.Remove(m);
        OnEnemyRemoved?.Invoke();
    }

    public static void RemoveAllEnemies()
    {
        Enemies.Clear();
    }
}
