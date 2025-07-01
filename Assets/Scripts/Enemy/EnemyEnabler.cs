using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEnabler : MonoBehaviour
{
    GameObject enemy;
    AudioSource source;
    NavMeshAgent agent;
    [SerializeField]
    float audioWaitTime = 2f;
    bool waitingOnAudio = false;
    bool gamePaused = false;

    private void OnEnable()
    {
        EnemiesInfo.OnHideEnemies += HideEnemy;
        EnemiesInfo.OnShowEnemies += ShowEnemy;
        EnemiesInfo.OnEnemyObjectRemoved += RemovedEnemy;
        GameStateActions.OnGamePause += Paused;
    }

    private void OnDisable()
    {
        EnemiesInfo.OnHideEnemies -= HideEnemy;
        EnemiesInfo.OnShowEnemies -= ShowEnemy;
        EnemiesInfo.OnEnemyObjectRemoved -= RemovedEnemy;
        GameStateActions.OnGamePause -= Paused;
    }

    private void Awake()
    {
        enemy = transform.GetChild(0).gameObject;
        source = gameObject.GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        source.enabled = false;
        agent.isStopped = true;
    }

    void Paused(bool paused)
    {
       
        if (paused)
        {
            //Debug.Log("audio pause");
            agent.isStopped = true;
            source.enabled = false;
            gamePaused = true;
        }
        else 
        {        
            if (GameStateActions.mapOpened && EnemiesInfo.EnemyActive(transform.GetChild(0).gameObject))
            {
               // Debug.Log("audio unpause");
                source.enabled = true;
                agent.isStopped = false;
            }
            gamePaused = false;   
        }
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
            agent.isStopped = false;
        }

    }

    void HideEnemy()
    {
        enemy?.SetActive(false);
        agent.isStopped = true;
        StartCoroutine(WaitForAudio());
    }

    IEnumerator WaitForAudio()
    {
        waitingOnAudio = true;
       // Debug.Log("coroutine started");
        yield return new WaitForSeconds(audioWaitTime);
        waitingOnAudio = false;
        source.enabled = false;
    }

    private void Update()
    {
        if (!EnemiesInfo.EnemyActive(enemy) && !waitingOnAudio)
        {
            source.enabled = false;
            agent.isStopped = true;
        }

        if (enemy.activeSelf && !source.enabled && !gamePaused)
        {
            source.enabled = true;
            agent.isStopped = false;
        }
    }
}
