using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState _currentState;

    private Dictionary<Type, IState> _states = new Dictionary<Type, IState>();

    #region StateMachineConstructor
    public StateMachine(IEnumerable<IState> states)
    {
        foreach (var state in states)
        {
            if (!_states.ContainsKey(state.GetType()))
            {
                _states.Add(state.GetType(), state);
            }
        }
    }
    #endregion

    public void Update()
    {
        _currentState?.OnUpdate();
    }

    public void SetState(IState state)
    {
        if (state == _currentState)
            return;

        _currentState?.OnExit();
        _currentState = state;
        _currentState?.OnEnter();
    }

    public void ChangeState(Type nextState)
    {
        if (nextState == null)
            return;

        if (_states.TryGetValue(nextState, out IState state))
        {
            SetState(state);
        }
    }

    public IState GetState(Type stateType)
    {
        return _states.TryGetValue(stateType, out var state) ? state : null;
    }
}
