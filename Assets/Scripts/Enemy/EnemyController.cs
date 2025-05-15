using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowPath))]
public class EnemyController : MonoBehaviour
{
    EnemyStateMachine fsm;
    [SerializeField]
    EnemyStateMachine.State startState;
    [SerializeField]
    Dictionary<EnemyStateMachine.State, float> stateSpeeds = new Dictionary<EnemyStateMachine.State, float>();

    void Start()
    {
        fsm = new EnemyStateMachine(gameObject.GetComponent<FollowPath>());
        fsm.AddState(EnemyStateMachine.State.Docile, new DocileState());
        fsm.AddState(EnemyStateMachine.State.Aggressive, new AggressiveState());
        fsm.AddState(EnemyStateMachine.State.Enraged, new EnragedState());

        fsm.SetStartState(startState);
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
    }
}
