
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowPath))]

public class EnemyController : MonoBehaviour
{
    FollowPath followPath;
    public EnemyStateMachine fsm;

    [SerializeField]
    EnemyStateMachine.State startState;

    CapsuleCollider col;

    [SerializeField]
    float enragedAttackRange;
    [SerializeField]
    float decoyDestroySpeedIncrease = 2;
    [SerializeField]
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
    }

    private void OnEnable()
    {
        EnemiesInfo.OnEnemyObjectRemoved += DestroyEnemy;
        EnemiesInfo.OnDecoyHit += DecoyDestroyed;      
    }

    private void OnDisable()
    {     
        EnemiesInfo.OnDecoyHit -= DecoyDestroyed;
        EnemiesInfo.OnEnemyObjectRemoved -= DestroyEnemy;
    }

    void Start()
    {
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
        Debug.Log("Speed set to " + stateSpeeds[state]);
        fsm.UpdateSpeeds(stateSpeeds);
    }

    public void SetTarget(Transform target)
    {
        followPath.target = target;
    }

    public void DestroyEnemy(GameObject destroyed)
    {
        if (destroyed == gameObject)
        {
            col.enabled = false;
            followPath.enabled = false;
            this.enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 2f);
        }
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

        if (fsm != null)
        {
            fsm.Update();
        }

        if ((followPath.target.position - transform.position).magnitude < enragedAttackRange)
        {
            if (fsm.currentStateName == EnemyStateMachine.State.Enraged && !enragedAttacking)
            {
                Debug.Log("Let's destroy them <3");
                EnemiesInfo.OnEnragedAttacks?.Invoke(this.gameObject);
                enragedAttacking = true;
            }
        }

        if (enragedAttacking && fsm.currentStateName != EnemyStateMachine.State.Enraged)
        {
            enragedAttacking = false;
        }
    }
}


