using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverHitState : PlayerAttackState
{
    private float timer = 0;
    public OverHitState(PlayerAttack player, PlayerAttackStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    public override void EnterState()
    {
        timer = 0;
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        timer += Time.deltaTime;

        if (timer > player.overHitTime)
        {
            timer = 0f;
            player.OverHitGauge = 0f;
            player.AttackStateMachine.ChangeState(player.AttackState);
        }

        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
