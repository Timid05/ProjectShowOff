using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class EnemiesInfo
{
    static Dictionary<EnemyStateMachine, GameObject> Enemies = new Dictionary<EnemyStateMachine, GameObject>();
    static Dictionary<EnemyStateMachine, GameObject> cachedEnemies = new Dictionary<EnemyStateMachine, GameObject>();
    static Dictionary<EnemyStateMachine, Vector3> cachedEnemyPositions = new Dictionary<EnemyStateMachine, Vector3>();

    public static Action<EnemyStateMachine.State> OnStateChange;
    public static Action OnEnemyAdded;
    public static Action<GameObject> OnEnemyObjectRemoved;
    public static Action OnEnemyRemoved;
    public static Action<GameObject> OnEnragedAttacks;
    public static Action<GameObject> OnDecoyHit;
    public static Action<GameObject> OnDecoyDestroyed;
    public static Action OnHideEnemies;
    public static Action OnShowEnemies;

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

    public static void EnableEnemies()
    {
        foreach (GameObject enemy in Enemies.Values)
        {
            enemy.SetActive(true);
        }
    }

    public static void DisableEnemies()
    {
        foreach (GameObject enemy in Enemies.Values)
        {
            enemy.SetActive(false);
        }
    }

    public static bool EnemyActive(GameObject enemy)
    {
        if (Enemies.ContainsValue(enemy))
        {
            return true;
        }
        else { return false; }
    }

    public static void AddEnemy(EnemyStateMachine m, GameObject g)
    {
        Enemies.Add(m, g);
        if (!cachedEnemies.ContainsKey(m))
        {
            cachedEnemies.Add(m, g);
        }
        if (!cachedEnemyPositions.ContainsKey(m))
        {
            cachedEnemyPositions.Add(m, g.transform.position);
        }
        OnEnemyAdded?.Invoke();
       // Debug.Log("Enemy Added");
    }

    public static void EnableCachedEnemies()
    {
        foreach (EnemyStateMachine m in cachedEnemies.Keys)
        {
            Enemies.Add(m, cachedEnemies[m]);
            Enemies[m].transform.position = cachedEnemyPositions[m];
            Enemies[m].SetActive(true);
        }
    }

    public static void RemoveEnemy(EnemyStateMachine m)
    {
        if (Enemies.ContainsKey(m))
        {
            GameObject toRemoveG = Enemies[m];
            Enemies.Remove(m);
            //Debug.Log("Enemy Removed");
            OnEnemyObjectRemoved?.Invoke(toRemoveG);
            OnEnemyRemoved?.Invoke();
        }
    }

    public static void RemoveAllEnemies()
    {
        //Debug.Log("Removing all enemies");
        for (int i = Enemies.Count - 1; i >= 0; i--)
        {
            RemoveEnemy(GetEnemyStateMachines()[i]);
        }
    }
}
