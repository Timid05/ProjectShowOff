
using UnityEngine;
using UnityEngine.AI;

public class HellHoundFightBehavior : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;

    [SerializeField, Tooltip("The minimum time in between the hellhound's growls")]
    float minGrowlInterval;
    [SerializeField, Tooltip("The maximum time in between the hellhound's growls")]
    float maxGrowlInterval;
    [SerializeField, Tooltip("The distance from the player's position that the hellhound growls/starts charging from")]
    float growlDistance;
    [SerializeField, Tooltip("The distance from the player's position that the hellhound pounces from")]
    float pounceDistance;
    [SerializeField, Tooltip("The speed at which the hellhound charges towards the player")]
    float chargeSpeed;
    [SerializeField, Tooltip("The amount of growls/barks the hellhound performs before it starts charging")]
    int growlsBeforeAttacking;
    [SerializeField, Tooltip("The amount of times the hellhound needs to be flashbanged before it gets killed")]
    int flashesUntilKilled;
    [SerializeField, Tooltip("The damage the hellhound does to the player when it gets off a succesful attack")]
    int attackDamage;

    bool fightOngoing = false;
    float currentInterval = 0;
    float lastIntervalTime = 0;
    int currentGrowls = 0;
    int currentFlashes = 0;
    bool charging = false;
    bool pouncing = false;
    bool dead = false;

    private void OnEnable()
    {
        HellhoundActions.OnHellhoundFightTriggered += StartFight;
        HellhoundActions.OnHellhoundFlashed += Flashed;
        HellhoundAnimations.OnPounceAnimDone += EndPounce;
        PlayerActions.OnPlayerDead += Disable;
    }

    private void OnDisable()
    {
        HellhoundActions.OnHellhoundFightTriggered -= StartFight;
        HellhoundActions.OnHellhoundFlashed -= Flashed;
        HellhoundAnimations.OnPounceAnimDone -= EndPounce;
        PlayerActions.OnPlayerDead -= Disable;
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
        HellhoundActions.hellhoundFightOngoing = true;
        fightOngoing = true;
        currentGrowls = 0;
        currentFlashes = 0;
    }

    private void Disable()
    {
        Disappear();
        fightOngoing = false;
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

    private void Flashed()
    {
        Disappear();
        agent.isStopped = true;
        charging = false;
        pouncing = false;
        currentFlashes++;
    }

    private void Charge()
    {
        HellhoundActions.OnHellhoundFlashable?.Invoke(true);
        transform.position = GetRandomPositionAroundPlayer();
        transform.LookAt(player.position);
        Appear();
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    private void Death()
    {
        agent.isStopped = true;
        dead = true;
        charging = false;
        pouncing = false;
        HellhoundActions.OnHellhoundDeath?.Invoke();
        Disappear();
        Destroy(gameObject, 3f);
    }

    private void Pounce()
    {
        HellhoundActions.OnPounce?.Invoke();
        agent.isStopped = true;
    }

    private void EndPounce()
    {
        if (pouncing)
        {
            pouncing = false;
            PlayerActions.OnPlayerDamaged?.Invoke(attackDamage);
            HellhoundActions.OnHellhoundFlashable?.Invoke(false);
            PlayerActions.OnPlayerHitBy?.Invoke(gameObject);
            Disappear();
        }
    }

    private void Update()
    {
        if (flashesUntilKilled == currentFlashes && !dead)
        {
            Death();
            return;
        }


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
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private bool InPounceDistance()
    {
        return Mathf.Abs(Vector3.Distance(transform.position, player.position)) <= pounceDistance;
    }

    private void Appear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}


