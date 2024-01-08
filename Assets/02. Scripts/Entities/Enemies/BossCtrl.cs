using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public abstract class BossCtrl : Entity
{
    #region
    //���� �� ��ٸ��� �ð�
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

    //���� ��� �ڵ�, ���� ���ڷ� �Է� ������ ����� �ڵ常 �����.
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
        //�ִϸ��̼� ���̾� ��ȯ(���� �������� ��ȯ)
        yield return ChangeAnimLayer(nowPatternIdx + 1, 0.1f, true);
        yield return StartCoroutine(patternList[nowPatternIdx].Invoke());
        //�ִϸ��̼� ���̾� ��ȯ(Idle �������)
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
