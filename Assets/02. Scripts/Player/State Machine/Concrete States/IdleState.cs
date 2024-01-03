using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerAttackState
{
    public IdleState(PlayerAttack player, PlayerAttackStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (player.OverHitGauge > 0)
        {
            player.OverHitGauge -= Time.deltaTime * 20f;
            Debug.Log($"<color=blue>Over Hit Cooldown</color>.\nOver Hit Gauge : {player.OverHitGauge.ToString("N2")}");
        }

        if (player.GetNearestTarget() != null)
        {
            player.AttackStateMachine.ChangeState(player.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
