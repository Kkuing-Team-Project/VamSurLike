using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerAttackStateMachine
{
    public PlayerAttackState CurrentPlayerAttackState { get; set; }

    public void Initialize(PlayerAttackState startingState)
    {
        CurrentPlayerAttackState = startingState;
        CurrentPlayerAttackState.EnterState();
    }

    public void ChangeState(PlayerAttackState newState)
    {
        CurrentPlayerAttackState.ExitState();
        CurrentPlayerAttackState = newState;
        CurrentPlayerAttackState.EnterState();
    }
}
