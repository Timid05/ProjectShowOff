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
        fsm = sM;
        followPath = f;

        followPath.navmeshAgent.speed = fsm.GetSpeed(state);
    }

    public void Exit()
    {
    }


    public void Update()
    {

        if (followPath.followType != FollowPath.FollowType.BackAndForth)
        {
            followPath.followType = FollowPath.FollowType.BackAndForth;
        }
    }

}
