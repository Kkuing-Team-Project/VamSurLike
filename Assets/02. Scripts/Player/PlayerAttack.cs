using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region State Machine Variables

    public PlayerAttackStateMachine AttackStateMachine { get; set; }
    public IdleState IdleState { get; set; }
    public AttackState AttackState { get; set; }
    public OverHitState OverHitState { get; set; }

    #endregion

    // 적 감지 범위
    public float detectRange = 5f;

    // 오버히트 지속시간
    public float overHitTime = 5f;

    // 공격 딜레이
    public float attackDelay = 0.2f;

    public ObjectPool BulletObjectPool;

    // 오버히트 게이지
    public float OverHitGauge { get; set; } = 0;

    private void Awake()
    {
        AttackStateMachine = new PlayerAttackStateMachine();

        IdleState = new IdleState(this, AttackStateMachine);
        AttackState = new AttackState(this, AttackStateMachine);
        OverHitState = new OverHitState(this, AttackStateMachine);
    }

    private void Start()
    {
        AttackStateMachine.Initialize(AttackState);
    }

    private void Update()
    {
        AttackStateMachine.CurrentPlayerAttackState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        AttackStateMachine.CurrentPlayerAttackState.PhysicsUpdate();   
    }

    public Transform GetNearestTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, detectRange, LayerMask.GetMask("ENEMY"));

        if (targets.Length == 0) return null;

        Transform target = targets[0].transform;

        for (int i = 0; i < targets.Length; i++)
        {
            if (Vector3.Distance(transform.position, targets[i].transform.position) < Vector3.Distance(transform.position, target.position))
            {
                target = targets[i].transform;
            }
        }
        return target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        
        Gizmos.DrawWireSphere(transform.position, detectRange);

        if (GetNearestTarget() != null) 
        {
            Gizmos.DrawLine(transform.position, GetNearestTarget().position);
        }
    }
}
