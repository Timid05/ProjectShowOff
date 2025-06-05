using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WitteWievenDecoy : MonoBehaviour
{
    CapsuleCollider col;
    MeshRenderer mr;

    [SerializeField]
    Transform target;
    [SerializeField]
    float decoySpeed;
    NavMeshAgent agent;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        col = GetComponent<CapsuleCollider>();
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

    public void Disappear()
    {
        mr.enabled = false;
        col.enabled = false;
        EnemiesInfo.OnDecoyDestroyed?.Invoke(gameObject);
    }

    void MoveToTarget()
    {
        if (agent.destination != target.position)
        {
            agent.SetDestination(target.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemiesInfo.OnDecoyHit?.Invoke(gameObject);
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 2f);        
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
