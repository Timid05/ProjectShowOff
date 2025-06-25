using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    Transform tanfanaTransform;
    [SerializeField]
    float respawnDistance = 5;

    private void OnEnable()
    {
        GameStateActions.OnRespawnEnemies += RespawnPosition;
        CameraActions.OnTanfanaCamInit += AssignTanfanaTransform;
    }

    private void OnDisable()
    {
        GameStateActions.OnRespawnEnemies -= RespawnPosition;
        CameraActions.OnTanfanaCamInit -= AssignTanfanaTransform;
    }

    void RespawnPosition()
    {
        if (tanfanaTransform != null)
        {
            transform.position = tanfanaTransform.position + tanfanaTransform.forward * respawnDistance;
        }
    }

    void AssignTanfanaTransform(GameObject tanfana)
    {
        Debug.Log("assigned");
        tanfanaTransform = tanfana.transform.parent;
    }
}
