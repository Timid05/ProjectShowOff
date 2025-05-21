using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    FollowPath followPath;
    Dictionary<State, float> stateSpeeds;
    FogController fog;
    List<float> fogDistances;

    public EnemyStateMachine(FollowPath f, Dictionary<State, float> speeds, FogController fogs, List<float> fogD)
    {
        followPath = f;
        stateSpeeds = speeds;
        fog = fogs;
        fogDistances = fogD;
    }

    public enum State { Docile, Aggressive, Enraged }
    public State currenStatename;
    private Dictionary<State, IEnemyState> states = new Dictionary<State, IEnemyState>();
    public IEnemyState currentState;

    public void SetStartState(State state)
    {
        if (states[state] != null)
        {
            currentState = states[state];
            currenStatename = state;
        }
        else
        {
            Debug.LogError("State machine does not contain a script for state: " + state);
        }
        currentState.Enter(this, followPath);
    }

    public float GetSpeed(State state)
    {
        return stateSpeeds[state];
    }

    public void SetFog(State state)
    {
        EnemiesInfo.OnStateChangeFog(states[state], fogDistances[(int)state]);
    }

    public void UpdateSpeeds(Dictionary<State, float> speeds)
    {
        stateSpeeds = speeds;
        Debug.Log("fsm speeds updated");
    }

    public void UpdateFogDistances(List<float> fogD)
    {
        fogDistances = fogD;
        Debug.Log("fsm fogs updated");
    }

    public void SetState(State state)
    {
        if (state == currenStatename) return;

        Debug.Log("Setting state to " + state);
        currentState?.Exit();

        if (states[state] != null)
        {
            currentState = states[state];
            currenStatename = state;
        }
        else
        {
            Debug.LogError("State machine does not contain a script for state: " + state);
        }
        currentState.Enter(this, followPath);
    }

    public void AddState(State stateName, IEnemyState stateScript)
    {
        states[stateName] = stateScript;
    }

    public IEnemyState GetState(State stateName)
    {
        return states[stateName];
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

}
