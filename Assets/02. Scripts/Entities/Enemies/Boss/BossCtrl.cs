using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public abstract class BossCtrl : Entity
{
    #region
    //ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½Ù¸ï¿½ï¿½ï¿?ï¿½Ã°ï¿½
    public float waitTime = 1;

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

    protected CinemachineImpulseSource cameraShakeSource;

    protected override void InitEntity()
    {
        base.InitEntity();
        if (playable == null)
            playable = FindObjectOfType<PlayableCtrl>();
        if (nav == null)
            nav = gameObject.GetComponent<NavMeshAgent>();
        patternIdx = 0;
        stat.SetDefault(StatType.MOVE_SPEED, 2f);
        gameObject.GetComponent<Collider>().enabled = true;
        cameraShakeSource = gameObject.GetComponent<CinemachineImpulseSource>();
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

        Vector3 lookAt = (playable.transform.position - transform.position).normalized;
        lookAt.y = 0;

        while(dT < time)
        {
            dT += Time.deltaTime;
            yield return null;
            float val = increasing ? dT / time : 1f - dT / time;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookAt), 180 * Time.deltaTime);
            animator.SetLayerWeight(layer, val);
            
        }

    }

    protected abstract void OnFinishPattern(int nowPatternIdx);

    protected override void OnEntityDied()
    {
        for (int i = 0; i < 50; i++)
        {
            float rand = Random.Range(0f, 360f);
            float x = Mathf.Sin(rand * Mathf.Rad2Deg);
            float z = Mathf.Cos(rand * Mathf.Rad2Deg);
            Vector3 position = transform.position + new Vector3(x, 0f, z) * Random.Range(0f, 10f);
            position.y = 1f;
            ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.Experience, transform.position).GetComponent<ExperienceGem>().ParabolicMovement(position);
        }
    }
}
