using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class DocileState : IEnemyState
{
    EnemyStateMachine fsm;
    EnemyStateMachine.State state = EnemyStateMachine.State.Docile;
    FollowPath followPath;

    public void Enter(EnemyStateMachine sM, FollowPath f)
    {
        Debug.Log("Entering Docile State");
        fsm = sM;
        followPath = f;

        followPath.speed = fsm.GetSpeed(state);
        fsm.SetFog(state);
    }

    public void Exit()
    {
        Debug.Log("Exiting Docile State");
    }


    public void Update()
    {

        if (followPath.followType != FollowPath.FollowType.BackAndForth)
        {
            followPath.followType = FollowPath.FollowType.BackAndForth;
        }
    }

}
