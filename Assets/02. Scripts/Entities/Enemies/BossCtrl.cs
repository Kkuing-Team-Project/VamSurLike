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

    protected int nowPatternIdx;

    protected delegate IEnumerator PatternDelegate();

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
        nowPatternIdx = Random.Range(0, patternList.Count);
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
        if (Vector3.Distance(origin, target) > stat.Get(StatType.ATTACK_DISTANCE) && attackPatternCor == null)
            BossMove();
        else
        {
            if (attackPatternCor == null)
                attackPatternCor = StartCoroutine(BossPatternCor());
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

    protected IEnumerator BossPatternCor()
    {
        //애니메이션 레이어 전환(공격 패턴으로 전환)
        yield return ChangeAnimLayer(nowPatternIdx + 1, 0.1f, true);
        yield return StartCoroutine(patternList[nowPatternIdx].Invoke());
        //애니메이션 레이어 전환(Idle 모션으로)
        yield return ChangeAnimLayer(nowPatternIdx + 1, waitTime, false);
        nowPatternIdx = Random.Range(0, patternList.Count);
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
}
