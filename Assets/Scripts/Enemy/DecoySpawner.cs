using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoySpawner : MonoBehaviour
{
    EnemyController enemyController;
    [SerializeField]
    GameObject decoyPrefab;
    [SerializeField]
    int decoySpawnCount;
    [SerializeField]
    float spawnRadius;
    [SerializeField]
    Transform target;

    private void OnEnable()
    {
        enemyController = GetComponent<EnemyController>();      
        EnemiesInfo.OnEnragedAttacks += HandleAttackAction;
    }

    private void Start()
    {
        target = enemyController.GetTarget();
    }

    private void OnDisable()
    {
        EnemiesInfo.OnEnragedAttacks -= HandleAttackAction;
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
            decoyScript.SetDecoyTarget(target);
        }
    }
    
    Vector3 GetRandomPosition()
    {
        float randomDistance = Random.Range(0f, spawnRadius);
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        return target.position + (new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * randomDistance);
    }
}
