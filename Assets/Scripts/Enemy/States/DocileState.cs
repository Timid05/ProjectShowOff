using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class DocileState : IEnemyState
{
    EnemyStateMachine fsm;
    EnemyStateMachine.State state = EnemyStateMachine.State.Docile;
    FollowPath followPath;
    float oldSpeed;

    public void Enter(EnemyStateMachine sM, FollowPath f)
    {
        Debug.Log("Entering Docile State");
        fsm = sM;
        followPath = f;

        followPath.speed = fsm.GetSpeed(state);
        oldSpeed = followPath.speed;
    }

    public void Exit()
    {
        Debug.Log("Exiting Docile State");
    }


    public void Update()
    {
        if (oldSpeed != fsm.GetSpeed(state))
        {
            followPath.speed = fsm.GetSpeed(state);
            oldSpeed = followPath.speed;
        }

        if (followPath.followType != FollowPath.FollowType.BackAndForth)
        {
            Debug.Log("Setting pathfollow behaviour");
            followPath.followType = FollowPath.FollowType.BackAndForth;
        }
    }

}
