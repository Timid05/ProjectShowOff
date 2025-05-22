using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UIElements;

public class FogController : MonoBehaviour
{
    [SerializeField, Range(0f, 3f)]
    float docileDistance = 0;
    [SerializeField, Range(0f, 3f)]
    float aggressiveDistance = 0;
    [SerializeField, Range(0f, 3f)]
    float enragedDistance = 0;

    public LocalVolumetricFog fog;
    public Transform target;
    [SerializeField]
    bool lockToTarget = false;

    float currentFogDistance;
    float oldFogDistance = 0;

    Vector3 oldPosition;

    private void OnEnable()
    {
        EnemiesInfo.OnStateChange += SetFogDistance;
        EnemiesInfo.OnEnemyRemoved += SetFogCheck;
        EnemiesInfo.OnEnemyAdded += SetFogCheck;
    }

    private void OnDisable()
    {
        EnemiesInfo.OnStateChange -= SetFogDistance;
        EnemiesInfo.OnEnemyRemoved -= SetFogCheck;
        EnemiesInfo.OnEnemyAdded -= SetFogCheck;
    }

    void Start()
    {
        fog.transform.position = target.position;
        oldPosition = target.position;
        currentFogDistance = fog.parameters.meanFreePath;
    }

    public void SetFogDistance(EnemyStateMachine.State state)
    {
        switch (state)
        {
            case EnemyStateMachine.State.Aggressive:
                if (!EnemiesInfo.HasEnragedEnemies()) { currentFogDistance = aggressiveDistance; Debug.Log("aggressive fog activated"); }
                break;
            case EnemyStateMachine.State.Enraged:
                currentFogDistance = enragedDistance;
                Debug.Log("enraged fog activated");
                break;
            case EnemyStateMachine.State.Docile:
                if (!EnemiesInfo.HasEnragedEnemies() && !EnemiesInfo.HasAggressiveEnemies()) { currentFogDistance = docileDistance; Debug.Log("docile fog activated"); }
                break;
            default:
                Debug.Log("passed state wasn't recognized");
                break;
        }
    }

    void SetFogCheck()
    {
        if (EnemiesInfo.HasEnragedEnemies())
        {
            currentFogDistance = enragedDistance;
            return;
        }
        else if (EnemiesInfo.HasAggressiveEnemies())
        {
            currentFogDistance = aggressiveDistance;
            return;
        }
        else if (EnemiesInfo.HasDocileEnemies())
        {
            currentFogDistance = docileDistance;
            return;
        }

        currentFogDistance = 3;
    }

    private void LerpFog(float newDistance, float t)
    {
        if (oldFogDistance == 0)
        {
            oldFogDistance = fog.parameters.meanFreePath;
        }

        fog.parameters.meanFreePath = Mathf.Lerp(oldFogDistance, currentFogDistance, t);
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

    float lerpT = 0;
    void Update()
    {
        if (target != null && lockToTarget)
        {
            UpdatePosition();
        }

        if (fog.parameters.meanFreePath != currentFogDistance)
        {
            lerpT += 0.02f;
            LerpFog(currentFogDistance, lerpT);
        }
        else if (lerpT != 0)
        {
            lerpT = 0;
            oldFogDistance = 0;
        }
    }
}
