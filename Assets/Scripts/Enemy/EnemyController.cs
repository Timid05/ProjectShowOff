
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowPath))]

public class EnemyController : MonoBehaviour
{
    FollowPath followPath;
    public EnemyStateMachine fsm;

    [SerializeField, Tooltip("The state this WW will start in")]
    EnemyStateMachine.State startState;

    CapsuleCollider col;

    [SerializeField, Tooltip("The amount of damage this WW does when attacking")]
    int attackDamage;
    [SerializeField, Tooltip("The distance from the player at which an enraged WW will start it's decoy attack behavior")]
    float enragedAttackRange;
    [SerializeField, Tooltip("The speed increase an enraged WW gets when one of its decoys gets destroyed")]
    float decoyDestroySpeedIncrease = 2;
    [SerializeField, Tooltip("The maximum distance the player can be displaced when a enraged WW attacks them")]
    float targetDisplacementRange = 10;
    bool enragedAttacking;

    [HideInInspector]
    public List<EnemyStateMachine.State> states = new List<EnemyStateMachine.State>();

    [HideInInspector]
    public List<float> speeds = new List<float>();
    public Dictionary<EnemyStateMachine.State, float> stateSpeeds = new Dictionary<EnemyStateMachine.State, float>();

    private void Awake()
    {
        followPath = GetComponent<FollowPath>();
        col = GetComponent<CapsuleCollider>();

        foreach (EnemyStateMachine.State state in states)
        {
            stateSpeeds[state] = speeds[(int)state];
        }

        fsm = new EnemyStateMachine(followPath, stateSpeeds);
        EnemiesInfo.AddEnemy(fsm, gameObject);
        fsm.AddState(EnemyStateMachine.State.Docile, new DocileState());
        fsm.AddState(EnemyStateMachine.State.Aggressive, new AggressiveState());
        fsm.AddState(EnemyStateMachine.State.Enraged, new EnragedState());

        fsm.SetStartState(startState);
    }

    private void OnEnable()
    {
        EnemiesInfo.OnDecoyHit += DecoyDestroyed;
        GameStateActions.OnChoiceMade += ChoiceMade;
    }

    private void OnDisable()
    {
        EnemiesInfo.OnDecoyHit -= DecoyDestroyed;
        GameStateActions.OnChoiceMade -= ChoiceMade;
    }

    private void DecoyDestroyed(GameObject decoy)
    {
        if (decoy.transform.IsChildOf(transform))
        {
            followPath.navmeshAgent.speed += decoyDestroySpeedIncrease;
        }
    }

    public Transform GetTarget()
    {
        return followPath.target;
    }

    public void EditStateSpeed(EnemyStateMachine.State state, float speed)
    {
        stateSpeeds[state] = speed;
       // Debug.Log("Speed set to " + stateSpeeds[state]);
        fsm.UpdateSpeeds(stateSpeeds);
    }

    public void SetTarget(Transform target)
    {
        followPath.target = target;
    }

    public void UpdateSpeeds()
    {
        fsm.UpdateSpeeds(stateSpeeds);
    }

    void ChoiceMade(bool choice)
    {
        if (!choice)
        {
           // Debug.Log("enraged");
            fsm.SetState(EnemyStateMachine.State.Enraged);
            GameStateActions.acceptedChoice = false;
        }
        else
        {
           // Debug.Log("docile");
            fsm.SetState(EnemyStateMachine.State.Docile);
            GameStateActions.acceptedChoice = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && followPath.followType == FollowPath.FollowType.Target)
        {
            PlayerActions.OnPlayerHit?.Invoke();
            PlayerActions.OnPlayerDamaged?.Invoke(attackDamage);
            PlayerActions.OnPlayerHitBy?.Invoke(gameObject);
            if (fsm.currentStateName == EnemyStateMachine.State.Enraged)
            {
                DisplaceTarget();
            }
            if (col.enabled)
            {
                EnemiesInfo.RemoveEnemy(fsm);
            }

        }
    }

    void DisplaceTarget()
    {
        float randomAngle = Random.Range(0f, 360f);
        float randomDistance = Random.Range(targetDisplacementRange / 2, targetDisplacementRange);
        GetTarget().position += new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle) * randomDistance);
    }


    void Update()
    {
        if (fsm != null)
        {
            fsm.Update();
        }


        if ((followPath.target.position - transform.position).magnitude < enragedAttackRange)
        {
            if (fsm.currentStateName == EnemyStateMachine.State.Enraged && !enragedAttacking)
            {
                followPath.followType = FollowPath.FollowType.Target;
                EnemiesInfo.OnEnragedAttacks?.Invoke(this.gameObject);
                enragedAttacking = true;
            }

            if (fsm.currentStateName == EnemyStateMachine.State.Aggressive)
            {
                if (followPath.followType != FollowPath.FollowType.Target)
                {
                    followPath.followType = FollowPath.FollowType.Target;
                }

                if (!GameStateActions.firstEnemyEncountered)
                {
                    GameStateActions.OnFirstEnemyEncounter?.Invoke();
                    GameStateActions.firstEnemyEncountered = true;
                }
            }
        }
        else
        {
            if (followPath.followType != FollowPath.FollowType.BackAndForth)
            {
                followPath.followType = FollowPath.FollowType.BackAndForth;
            }
            if (enragedAttacking)
            {
                enragedAttacking = false;
            }
        }

        if (enragedAttacking && fsm.currentStateName != EnemyStateMachine.State.Enraged)
        {
            enragedAttacking = false;
        }
    }
}


