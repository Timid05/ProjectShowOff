using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnabler : MonoBehaviour
{
    GameObject enemy;

    private void OnEnable()
    {
        EnemiesInfo.OnHideEnemies += HideEnemy;
        EnemiesInfo.OnShowEnemies += ShowEnemy;
    }

    private void OnDisable()
    {
        EnemiesInfo.OnHideEnemies -= HideEnemy;
        EnemiesInfo.OnShowEnemies -= ShowEnemy;
    }

    private void Awake()
    {
        enemy = transform.GetChild(0).gameObject;
    }

    void ShowEnemy()
    {
        if (EnemiesInfo.EnemyActive(enemy))
        {
            enemy?.SetActive(true);
        }

    }

    void HideEnemy()
    {
        enemy?.SetActive(false);
    }

}
