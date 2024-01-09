using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public abstract class BossCtrl : Entity
{
    #region
    //ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½Ù¸ï¿½ï¿½ï¿?ï¿½Ã°ï¿½
    public float waitTime;

    #endregion

    //ÆÐÅÏ ´ë¸®ÀÚ(ÇØ´ç ´ë¸®ÀÚ·Î ÆÐÅÏ Àü´Þ)
    protected delegate IEnumerator PatternDelegate();

    //ÇöÀç ÆÐÅÏ
    protected int patternIdx;

    //ÇÃ·¹ÀÌ¾î±îÁöÀÇ °Å¸®
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pattern">pattern parameter to register (variadic arguments)</param>
    protected void RegisterPatterns(params PatternDelegate[] pattern)
    {
        foreach (PatternDelegate patternDelegate in pattern)
        {
            patternList.Add(patternDelegate);
        }
    }

    protected override void UpdateEntity()
    {
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
        //¾Ö´Ï¸ÞÀÌ¼Ç ·¹ÀÌ¾î ÀüÈ¯(°ø°Ý ÆÐÅÏÀ¸·Î ÀüÈ¯)
        yield return ChangeAnimLayer(patternIdx + 1, 0.1f, true);
        yield return StartCoroutine(patternList[patternIdx].Invoke());
        //¾Ö´Ï¸ÞÀÌ¼Ç ·¹ÀÌ¾î ÀüÈ¯(Idle ¸ð¼ÇÀ¸·Î)
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
