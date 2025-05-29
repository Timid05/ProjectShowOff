using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class EnemiesInfo
{
    static Dictionary<EnemyStateMachine, GameObject> Enemies = new Dictionary<EnemyStateMachine, GameObject>();

    public static Action<EnemyStateMachine.State> OnStateChange;
    public static Action OnEnemyAdded;
    public static Action<GameObject> OnEnemyObjectRemoved;
    public static Action OnEnemyRemoved;
    public static Action<GameObject> OnEnragedAttacks;
    public static Action<GameObject> OnDecoyHit;
    public static Action<GameObject> OnDecoyDestroyed;

    public static bool HasAggressiveEnemies()
    {
        foreach (EnemyStateMachine m in Enemies.Keys)
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
        foreach (EnemyStateMachine m in Enemies.Keys)
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
        foreach (EnemyStateMachine m in Enemies.Keys)
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
        return Enemies.Keys.ToList<EnemyStateMachine>();
    }

    public static List<GameObject> GetEnemyGameObjects()
    {
        return Enemies.Values.ToList<GameObject>();
    }

    public static void AddEnemy(EnemyStateMachine m, GameObject g)
    {
        Enemies.Add(m, g);
        OnEnemyAdded?.Invoke();
        Debug.Log("Enemy Added");
    }

    public static void RemoveEnemy(EnemyStateMachine m)
    {
        GameObject toRemoveG = Enemies[m];
        Enemies.Remove(m);
        Debug.Log("Enemy Removed");
        OnEnemyObjectRemoved?.Invoke(toRemoveG);
        OnEnemyRemoved?.Invoke();     
    }

    public static void RemoveAllEnemies()
    {
        Debug.Log("Removing all enemies");
        for (int i = Enemies.Count - 1; i >= 0; i--)
        {
            RemoveEnemy(GetEnemyStateMachines()[i]);
        }
    }
}
