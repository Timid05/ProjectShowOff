using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(FollowPath))]
[RequireComponent (typeof(FogController))]

public class EnemyController : MonoBehaviour
{
    FollowPath followPath;
    FogController fogController;
    public EnemyStateMachine fsm;

    [SerializeField]
    EnemyStateMachine.State startState;

    [HideInInspector]
    public List<EnemyStateMachine.State> states = new List<EnemyStateMachine.State>();

    [HideInInspector]
    public List<float> speeds = new List<float>();
    public Dictionary<EnemyStateMachine.State, float> stateSpeeds= new Dictionary<EnemyStateMachine.State, float>();

    [HideInInspector]
    public List<float> fogs = new List<float>();

    private void Awake()
    {
        fogController = GetComponent<FogController>();
        followPath = GetComponent<FollowPath>();
    }

    void Start()
    {
        foreach (EnemyStateMachine.State state in states)
        {
            stateSpeeds[state] = speeds[(int)state];
        }

        fsm = new EnemyStateMachine(followPath, stateSpeeds, fogController, fogs);
        fsm.AddState(EnemyStateMachine.State.Docile, new DocileState());
        fsm.AddState(EnemyStateMachine.State.Aggressive, new AggressiveState());
        fsm.AddState(EnemyStateMachine.State.Enraged, new EnragedState());

        fsm.SetStartState(startState);
    }

    public void EditStateSpeed(EnemyStateMachine.State state, float speed)
    {
        stateSpeeds[state] = speed;
        Debug.Log("Speed set to " + stateSpeeds[state]);
        fsm.UpdateSpeeds(stateSpeeds);
    }

    public void AssignFog(LocalVolumetricFog fog, Transform target)
    {
        fogController.fog = fog;
        fogController.target = target;
    }

    public void SetTarget(Transform target)
    {
        followPath.target = target;
    }

    public void DestroyEnemy()
    {
        followPath.enabled = false;
        Destroy(gameObject, 0.5f);
    }

    public void UpdateSpeeds()
    {
        fsm.UpdateSpeeds(stateSpeeds);
    }

    public void UpdateFogDistances()
    {
        fsm.UpdateFogDistances(fogs);
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
    }
}


