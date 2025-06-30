using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyEnabler : MonoBehaviour
{
    GameObject enemy;
    AudioSource source;
    [SerializeField]
    float audioWaitTime = 2f;
    bool waitingOnAudio = false;

    private void OnEnable()
    {
        EnemiesInfo.OnHideEnemies += HideEnemy;
        EnemiesInfo.OnShowEnemies += ShowEnemy;
        EnemiesInfo.OnEnemyObjectRemoved += RemovedEnemy;
    }

    private void OnDisable()
    {
        EnemiesInfo.OnHideEnemies -= HideEnemy;
        EnemiesInfo.OnShowEnemies -= ShowEnemy;
        EnemiesInfo.OnEnemyObjectRemoved -= RemovedEnemy;
    }

    private void Awake()
    {
        enemy = transform.GetChild(0).gameObject;
        source = gameObject.GetComponent<AudioSource>();
        source.enabled = false;
    }

    void RemovedEnemy(GameObject removed)
    {
        if (removed == enemy)
        {
            HideEnemy();
        }
    }

    void ShowEnemy()
    {
        if (EnemiesInfo.EnemyActive(enemy))
        {
            enemy?.SetActive(true);
            source.enabled = true;
        }

    }

    void HideEnemy()
    {
        enemy?.SetActive(false);
        StartCoroutine(WaitForAudio());
    }

    IEnumerator WaitForAudio()
    {
        waitingOnAudio = true;
        Debug.Log("coroutine started");
        yield return new WaitForSeconds(audioWaitTime);
        waitingOnAudio = false;
        source.enabled = false;
    }

    private void Update()
    {
        if (!EnemiesInfo.EnemyActive(enemy) && !waitingOnAudio)
        {
            source.enabled = false;
        }

        if (enemy.activeSelf && !source.enabled)
        {
            source.enabled = true;
        }
    }
}
