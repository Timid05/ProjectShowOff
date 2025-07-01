using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoySpawner : MonoBehaviour
{
    List<GameObject> decoys = new List<GameObject>();
    EnemyController enemyController;


    [SerializeField]
    GameObject decoyPrefab;
    [SerializeField]
    int decoySpawnCount;
    [SerializeField]
    float minSpawnRadius;
    [SerializeField]
    float maxSpawnRadius;
    [SerializeField]
    Transform target;

    private void OnEnable()
    {
        enemyController = GetComponent<EnemyController>();
        EnemiesInfo.OnEnragedAttacks += HandleAttackAction;
        EnemiesInfo.OnEnemyObjectRemoved += DestroyDecoys;
    }

    private void Start()
    {
        target = enemyController.GetTarget();
    }

    private void OnDisable()
    {
        EnemiesInfo.OnEnragedAttacks -= HandleAttackAction;
        EnemiesInfo.OnEnemyObjectRemoved -= DestroyDecoys;
    }

    void HandleAttackAction(GameObject attacker)
    {
        if (attacker == gameObject)
        {
            SpawnDecoys();
        }
    }

    void SpawnDecoys()
    {
        for (int i = 0; i < decoySpawnCount; i++)
        {
            GameObject newDecoy = Instantiate(decoyPrefab, GetRandomPosition(), Quaternion.identity, transform);
            WitteWievenDecoy decoyScript = newDecoy.GetComponent<WitteWievenDecoy>();
            decoys.Add(newDecoy);
            decoyScript.SetDecoyTarget(target);
        }
    }

    void DestroyDecoys(GameObject removedEnemy)
    {
       // Debug.Log("Destroyed enemy decoy disappearance");
        if (removedEnemy == gameObject && enemyController.fsm.currentStateName == EnemyStateMachine.State.Enraged)
        {
            for (int i = decoys.Count - 1; i >= 0; i--)
            {
                if (decoys[i] != null)
                {
                    decoys[i].GetComponent<WitteWievenDecoy>().Disappear();
                }
                decoys.RemoveAt(i);
            }
            //Debug.Log("Destroyed decoys");
        }
    }

    Vector3 GetRandomPosition()
    {
        float randomDistance = Random.Range(minSpawnRadius, maxSpawnRadius);
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        return target.position + (new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * randomDistance);
    }

    private void Update()
    {
        if (enemyController.fsm.currentStateName != EnemyStateMachine.State.Enraged && decoys.Count != 0)
        {
            //Debug.Log("State change decoy disappearance");
            for (int i = decoys.Count - 1; i >= 0; i--)
            {
                decoys[i].GetComponent<WitteWievenDecoy>().Disappear();
                decoys.RemoveAt(i);
            }
            //Debug.Log("Destroyed decoys");
        }
    }
}
