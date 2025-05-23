using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Events;

[RequireComponent(typeof(FollowPath))]

public class EnemyController : MonoBehaviour
{
    FollowPath followPath;
    public EnemyStateMachine fsm;

    [SerializeField]
    EnemyStateMachine.State startState;

    [SerializeField]
    float enragedAttackRange;
    bool enragedAttacking;

    [HideInInspector]
    public List<EnemyStateMachine.State> states = new List<EnemyStateMachine.State>();

    [HideInInspector]
    public List<float> speeds = new List<float>();
    public Dictionary<EnemyStateMachine.State, float> stateSpeeds = new Dictionary<EnemyStateMachine.State, float>();

    private void Awake()
    {
        followPath = GetComponent<FollowPath>();
    }

    private void OnEnable()
    {
        PlayerActions.OnPlayerDead += DestroyEnemy;
    }

    private void OnDisable()
    {
        PlayerActions.OnPlayerDead -= DestroyEnemy;
        EnemiesInfo.RemoveStateMachine(fsm);
    }

    void Start()
    {
        foreach (EnemyStateMachine.State state in states)
        {
            stateSpeeds[state] = speeds[(int)state];
        }

        fsm = new EnemyStateMachine(followPath, stateSpeeds);
        EnemiesInfo.AddStateMachine(fsm);
        fsm.AddState(EnemyStateMachine.State.Docile, new DocileState());
        fsm.AddState(EnemyStateMachine.State.Aggressive, new AggressiveState());
        fsm.AddState(EnemyStateMachine.State.Enraged, new EnragedState());

        fsm.SetStartState(startState);
    }

    public Transform GetTarget()
    {
        return followPath.target;
    }

    public void EditStateSpeed(EnemyStateMachine.State state, float speed)
    {
        stateSpeeds[state] = speed;
        Debug.Log("Speed set to " + stateSpeeds[state]);
        fsm.UpdateSpeeds(stateSpeeds);
    }

    public void SetTarget(Transform target)
    {
        followPath.target = target;
    }

    public void DestroyEnemy()
    {
        followPath.enabled = false;
       // Destroy(gameObject, 0.5f);
    }

    public void UpdateSpeeds()
    {
        fsm.UpdateSpeeds(stateSpeeds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && followPath.followType == FollowPath.FollowType.Target)
        {
            PlayerActions.OnPlayerHit?.Invoke();
            PlayerActions.OnPlayerDamaged?.Invoke(3);
            DestroyEnemy();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            fsm.SetState(EnemyStateMachine.State.Aggressive);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            fsm.SetState(EnemyStateMachine.State.Docile);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            fsm.SetState(EnemyStateMachine.State.Enraged);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Editing speed");
            EditStateSpeed(EnemyStateMachine.State.Enraged, 1);
        }

        if (fsm != null)
        {
            fsm.Update();
        }

        if ((followPath.target.position - transform.position).sqrMagnitude < enragedAttackRange)
        {
            if (fsm.currentStateName == EnemyStateMachine.State.Enraged && !enragedAttacking)
            {
                Debug.Log("Let's destroy them <3");
                EnemiesInfo.OnEnragedAttacks?.Invoke(this.gameObject);
                enragedAttacking = true;
            }
        }
    }
}


