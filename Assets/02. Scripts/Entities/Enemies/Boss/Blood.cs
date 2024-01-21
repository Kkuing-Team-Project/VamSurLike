using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : BossCtrl, IPoolable
{
    public ObjectPool pool { get; set; }
    public Transform[] patternTr;
    public float pattern1Rad = 3;
    public Vector3 pattern2Box;
    public float pattern3Rad = 5;
    protected override void InitEntity()
    {
        base.InitEntity();
        RegisterPatterns(new PatternDelegate(Pattern1), new PatternDelegate(Pattern2), new PatternDelegate(Pattern3));
    }

    protected override void OnFinishPattern(int nowPatternIdx)
    {
        if (nowPatternIdx < patternList.Count - 1)
            patternIdx++;
        else
            patternIdx = 0;
    }

    private IEnumerator Pattern1()
    {
        animator.SetTrigger("Pattern1");
        yield return new WaitUntil(() => IsAnimationClipPlaying("Wait", patternIdx + 1) == true);
    }

    private IEnumerator Pattern2()
    {
        animator.SetTrigger("Pattern2");
        yield return new WaitUntil(() => IsAnimationClipPlaying("Wait", patternIdx + 1) == true);
    }

    private IEnumerator Pattern3()
    {
        animator.SetTrigger("Pattern3");
        yield return new WaitUntil(() => IsAnimationClipPlaying("Wait", patternIdx + 1) == true);
    }

    private IEnumerator DeathCor()
    {
        yield return ChangeAnimLayer(patternIdx + 1, 0.5f, false);
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(3f);

        ReturnObject();
    }

    public void Pattern1Attack()
    {
        HitEffect effect = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.HitParticle, patternTr[0].position).GetComponent<HitEffect>();
        Collider[] col = Physics.OverlapSphere(patternTr[0].position, pattern1Rad, 1 << LayerMask.NameToLayer("PLAYER"));
        cameraShakeSource.GenerateImpulse();
        if (col.Length > 0)
        {
            col[0].GetComponent<Entity>().TakeDamage(this, stat.Get(StatType.DAMAGE));
        }
    }

    
    public void Pattern2Attack()
    {
        LightningBoltEffect effect = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.LightningBolt, patternTr[0].position).GetComponent<LightningBoltEffect>();
        effect.transform.localScale = Vector3.one * 1.5f;
        Collider[] col = Physics.OverlapBox(patternTr[1].position, pattern2Box / 2, patternTr[1].rotation, 1 << LayerMask.NameToLayer("PLAYER"));
        cameraShakeSource.GenerateImpulse();
        if (col.Length > 0)
        {
            col[0].GetComponent<Entity>().TakeDamage(this, stat.Get(StatType.DAMAGE));
        }
    }

    
    public void Pattern3Attack()
    {
        HitEffect effect = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.HitParticle, patternTr[2].position).GetComponent<HitEffect>();
        effect.transform.localScale = Vector3.one * 4f;
        Collider[] col = Physics.OverlapSphere(patternTr[2].position, pattern3Rad, 1 << LayerMask.NameToLayer("PLAYER"));
        cameraShakeSource.GenerateImpulse();
        if (col.Length > 0)
        {
            col[0].GetComponent<Entity>().TakeDamage(this, stat.Get(StatType.DAMAGE));
        }
    }

    

    protected override void OnEntityDied()
    {
        base.OnEntityDied();
        statusEffects.Add(new Stun(1, 5f, this));
        nav.isStopped = true;
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(DeathCor());
    }

    public void OnCreate()
    {
        pool = FindObjectOfType<ObjectPool>();

    }

    public void OnActivate()
    {
    }

    public void ReturnObject()
    {
        pool?.ReturnObject(gameObject, ObjectPool.ObjectType.Blood);
    }
}
