using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    Transform enemyTarget;
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    LocalVolumetricFog fog;
    [SerializeField]
    bool lockToTarget = false;

    [SerializeField]
    float spawnInterval = 10;
    [SerializeField]
    float spawnRadius = 10;
    float lastSpawnTime = 0;

    bool EnoughTimeElapsed()
    {
        if (Time.time - lastSpawnTime > spawnInterval)
        {
            lastSpawnTime = Time.time;
            return true;
        }
        else return false;
    }

    void spawnNewEnemy()
    {
        if (enemyPrefab != null)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity, gameObject.transform);
            if (enemyTarget != null)
            {
                newEnemy.gameObject.GetComponent<EnemyController>().SetTarget(enemyTarget);
                newEnemy.gameObject.GetComponent<EnemyController>().AssignFog(fog, enemyTarget);
            }
            else
            {
                Debug.Log("No enemy target was given, could not set target");
            }
        }
        else
        {
            Debug.Log("No target prefab was given, could not spawn enemy");
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float randomAngle = Random.Range(1f, 360f);
        float randomDistance = Random.Range(0f, spawnRadius);

        return new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).normalized * randomDistance;
    }

    private void Update()
    {
        if (EnoughTimeElapsed())
        {
            Debug.Log("Spawning new enemy");
            spawnNewEnemy();
        }

        if (lockToTarget)
        {
            transform.position = enemyTarget.position;
        }
    }

}
