using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public abstract class BossCtrl : Entity
{
    #region
    //패턴 후 기다리는 시간
    public float waitTime;

    #endregion

    //패턴 대리자(해당 대리자로 패턴 전달)
    protected delegate IEnumerator PatternDelegate();

    //현재 패턴
    protected int patternIdx;

    //플레이어까지의 거리
    protected float distToPlayer;

    //

    protected List<PatternDelegate> patternList = new List<PatternDelegate>();

    protected NavMeshAgent nav;
    protected PlayableCtrl playable;
    protected Transform enemyPool;

    protected Coroutine attackPatternCor;

    protected override void InitEntity()
    {
        base.InitEntity();
        if (playable == null)
            playable = FindObjectOfType<PlayableCtrl>();
        if (nav == null)
            nav = gameObject.GetComponent<NavMeshAgent>();
        if (enemyPool == null)
        {
            enemyPool = GameObject.Find("EnemyPool").transform;
            transform.SetParent(enemyPool);
        }
        patternIdx = 0;
    }

    //패턴 등록 코드, 가변 인자로 입력 받으며 등록한 코드만 실행됨.
    protected void RegisterPatterns(params PatternDelegate[] pattern)
    {
        foreach (PatternDelegate patternDelegate in pattern)
        {
            patternList.Add(patternDelegate);
        }
    }

    protected override void UpdateEntity()
    {
        base.UpdateEntity();
        var origin = transform.position;
        origin.y = 0;
        var target = playable.transform.position;
        target.y = 0;

        distToPlayer = Vector3.Distance(origin, target);
        if (distToPlayer > stat.Get(StatType.ATTACK_DISTANCE) && attackPatternCor == null)
            BossMove();
        else
        {
            if (attackPatternCor == null)
                attackPatternCor = StartCoroutine(BossPatternCor(patternIdx));
        }
        BossAnimation();
    }

    protected virtual void BossAnimation()
    {
        if(animator != null)
        {
            animator.SetFloat("Velocity", nav.velocity.normalized.magnitude);
        }
    }

    protected virtual void BossMove()
    {
        nav.speed = stat.Get(StatType.MOVE_SPEED);
        nav.stoppingDistance = stat.Get(StatType.ATTACK_DISTANCE);
        nav.SetDestination(playable.transform.position);
    }

    protected virtual IEnumerator BossPatternCor(int patternIdx)
    {
        //애니메이션 레이어 전환(공격 패턴으로 전환)
        yield return ChangeAnimLayer(patternIdx + 1, 0.1f, true);
        yield return StartCoroutine(patternList[patternIdx].Invoke());
        //애니메이션 레이어 전환(Idle 모션으로)
        yield return ChangeAnimLayer(patternIdx + 1, waitTime, false);
        OnFinishPattern(this.patternIdx);
        attackPatternCor = null;
    }

    protected IEnumerator ChangeAnimLayer(int layer, float time, bool increasing)
    {
        if (animator == null)
            yield break;

        float dT = 0;
        while(dT < time)
        {
            dT += Time.deltaTime;
            yield return null;
            float val = increasing ? dT / time : 1f - dT / time;

            animator.SetLayerWeight(layer, val);
        }

    }

    protected abstract void OnFinishPattern(int nowPatternIdx);
}
