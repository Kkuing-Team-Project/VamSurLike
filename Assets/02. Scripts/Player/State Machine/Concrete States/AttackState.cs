using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerAttackState
{
    private float timer;

    private float exitTimer;
    private float timeTillAttackExit = 1f;

    public AttackState(PlayerAttack player, PlayerAttackStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    public override void EnterState()
    {
        timer = 0;
        exitTimer = 0;
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // 오버히트 게이지가 100 이상일 때 OverHit State로 전환
        if (player.OverHitGauge >= 100f)
        {
            player.AttackStateMachine.ChangeState(player.OverHitState);
            Debug.Log($"<color=red>Over Hit!</color> {player.overHitTime}초간 Over Hit 상태 돌입.");
        }

        // 공격할 대상이 있을 때
        if (player.GetNearestTarget() != null)
        {
            timer += Time.deltaTime;

            if (timer > player.attackDelay)
            {
                timer = 0;

                Vector3 direction = player.GetNearestTarget().position - player.transform.position;

                GameObject.Instantiate<Rigidbody>(player.BulletPrefab, player.transform.position, Quaternion.identity).velocity = direction * 10f;

                player.OverHitGauge += 5f;
                Debug.Log($"<color=red>Over Hit Gauge</color> : {player.OverHitGauge}");
            }
        }

        // 공격할 대상이 없을 때
        else
        {
            exitTimer += Time.deltaTime;

            // 일정 시간이 지나면 Idle State로 전환
            if (exitTimer > timeTillAttackExit)
            {
                exitTimer = 0;
                player.AttackStateMachine.ChangeState(player.IdleState);
            }
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
