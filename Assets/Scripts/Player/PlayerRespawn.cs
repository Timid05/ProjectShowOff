using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    Transform tanfanaTransform;
    [SerializeField]
    Transform hellhoundTransform;
    [SerializeField]
    float tanfanaRespawnDistance = 5;
    [SerializeField]
    float hellhoundRespawnDistance = 10;
    Vector3 spawnPosition;

    private void OnEnable()
    {
        PlayerActions.OnPlayerRespawn += RespawnPosition;
        CameraActions.OnTanfanaCamInit += AssignTanfanaTransform;
    }

    private void OnDisable()
    {
        PlayerActions.OnPlayerRespawn -= RespawnPosition;
        CameraActions.OnTanfanaCamInit -= AssignTanfanaTransform;
    }

    private void Start()
    {
        spawnPosition = transform.position;
    }

    void RespawnPosition()
    {
        if (HellhoundActions.hellhoundFightOngoing)
        {
            if (hellhoundTransform != null)
            {
                transform.position = hellhoundTransform.position + hellhoundTransform.forward * hellhoundRespawnDistance; 
            }
        }
        else if (tanfanaTransform != null && GameStateActions.domeVisited)
        {
            transform.position = tanfanaTransform.position + tanfanaTransform.forward * tanfanaRespawnDistance;
        }
        else
        {
            transform.position = spawnPosition;
        }
    }

    void AssignTanfanaTransform(GameObject tanfana)
    {
        Debug.Log("assigned");
        tanfanaTransform = tanfana.transform.parent;
    }
}
