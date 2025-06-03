using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class HellHoundFightBehavior : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;

    [SerializeField]
    float minGrowlInterval, maxGrowlInterval, growlDistance, pounceDistance, chargeSpeed;
    [SerializeField]
    int growlsBeforeAttacking, flashesUntilKilled, attackDamage;

    bool fightOngoing = false;
    float currentInterval = 0;
    float lastIntervalTime = 0;
    int currentGrowls = 0;
    bool charging = false;
    bool pouncing = false;  

    private void OnEnable()
    {
        HellhoundActions.OnHellhoundFightTriggered += StartFight;
    }

    private void OnDisable()
    {
        HellhoundActions.OnHellhoundFightTriggered -= StartFight;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = chargeSpeed;
    }

    private void StartFight()
    {
        Disappear();
        fightOngoing = true;
    }

    private void AttackBehavior()
    {
        if (currentInterval == 0) { currentInterval = GetRandomInterval(); lastIntervalTime = Time.time; }

        if (Time.time - lastIntervalTime >= currentInterval)
        {
            currentGrowls++;
            if (currentGrowls == growlsBeforeAttacking + 1)
            {
                charging = true;
                HellhoundActions.OnCharge?.Invoke();
                Charge();
                currentInterval = 0;
                currentGrowls = 0;
                return;
            }

            transform.position = GetRandomPositionAroundPlayer();
            HellhoundActions.OnGrowlTriggered?.Invoke();   
            currentInterval = GetRandomInterval();
            lastIntervalTime = Time.time;
        }     
    }

    private void Charge()
    {
        transform.position = GetRandomPositionAroundPlayer();
        Appear();
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }


    private void Pounce()
    {
        HellhoundActions.OnPounce?.Invoke();
        PlayerActions.OnPlayerDamaged?.Invoke(attackDamage);
        agent.isStopped = true;
        pouncing = false;
        Disappear();
    }

    private void Update()
    {
        if (fightOngoing && !pouncing && !charging)
        {
            AttackBehavior();
        }

        if (charging)
        {
            agent.SetDestination(player.position);

            if (InPounceDistance())
            {
                charging = false;
                pouncing = true;
                Pounce();
            }
        }
    }

    private Vector3 GetRandomPositionAroundPlayer()
    {
        float randomAngle = Random.Range(0f, 360f);
        return player.position + new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * growlDistance;
    }

    private float GetRandomInterval()
    {
        return Random.Range(minGrowlInterval, maxGrowlInterval);
    }

    private void Disappear()
    {
       transform.GetChild(0).gameObject.SetActive(false);
    }

    private bool InPounceDistance()
    {
        return Mathf.Abs(Vector3.Distance(transform.position, player.position)) <= pounceDistance;
    }

    private void Appear()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}


