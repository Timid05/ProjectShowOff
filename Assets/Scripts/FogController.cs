using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UIElements;

public class FogController : MonoBehaviour
{
    public LocalVolumetricFog fog;
    public Transform target;

    Vector3 oldPosition;

    private void OnEnable()
    {
        EnemiesInfo.OnStateChangeFog = SetFogDistance;
    }

    void Start()
    {
        fog.transform.position = target.position;
        oldPosition = target.position;
    }

    public void SetFogDistance(IEnemyState state, float distance)
    {
        switch (state)
        {
            case AggressiveState:
                if (!EnemiesInfo.HasEnragedEnemies()) { fog.parameters.meanFreePath = distance; Debug.Log("aggressive fog activated"); }
                break;
            case EnragedState:
                fog.parameters.meanFreePath = distance;
                Debug.Log("enraged fog activated");
                break;
            case DocileState:
                if (!EnemiesInfo.HasEnragedEnemies() && !EnemiesInfo.HasAggressiveEnemies()) { fog.parameters.meanFreePath = distance; Debug.Log("docile fog activated"); }
                break;
            default:
                Debug.Log("passed state wasn't recognized");
                break;
        }
    }

    public void EnableFog()
    {
        fog.enabled = true;
    }

    public void DisableFog()
    {
        fog.enabled = false;
    }

    void UpdatePosition()
    {
        if (oldPosition != target.position)
        {
            fog.transform.position = target.position;
            oldPosition = target.position;
        }
    }

    void Update()
    {
        if (target != null)
        {
            UpdatePosition();
        }
    }
}
