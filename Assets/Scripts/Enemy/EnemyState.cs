using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    public void Enter(EnemyStateMachine sM, FollowPath f);
    public void Exit();
    public void Update();   
}
