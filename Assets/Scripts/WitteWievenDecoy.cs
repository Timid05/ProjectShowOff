using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WitteWievenDecoy : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    float decoySpeed;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = decoySpeed;
    }

    public void SetDecoyTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetDecoySpeed(float speed)
    {
        decoySpeed = speed;
        agent.speed = speed;
    }

    void MoveToTarget()
    {
        if (agent.destination != target.position)
        {
            agent.SetDestination(target.position);
        }
    }

    private void Update()
    {
        if (target != null)
        {
            MoveToTarget();
        }
    }
}
