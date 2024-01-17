using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : BossCtrl
{
    public Transform[] patternTr;
    public float pattern1Rad = 3;
    public Vector3 pattern2Box;
    public Vector3 pattern3Box;
    public float pattern4Rad = 3;

    protected override void InitEntity()
    {
        base.InitEntity();
        RegisterPatterns(new PatternDelegate(Pattern1), new PatternDelegate(Pattern2), new PatternDelegate(Pattern3), new PatternDelegate(Pattern4));
    }

    public IEnumerator Pattern1()
    {
        animator.SetTrigger("Pattern1");
        yield return new WaitUntil(() => IsAnimationClipPlaying("Wait", patternIdx + 1) == true);
    }

    public IEnumerator Pattern2()
    {
        animator.SetTrigger("Pattern2");
        yield return new WaitUntil(() => IsAnimationClipPlaying("Loop", patternIdx + 1) == true);
        yield return new WaitUntil(() => IsAnimationClipPlaying("Loop", patternIdx + 1) == false);
        Pattern2Attack();
    }
    public IEnumerator Pattern3()
    {
        animator.SetTrigger("Pattern3");
        yield return new WaitUntil(() => IsAnimationClipPlaying("Wait", patternIdx + 1) == true);
    }
    public IEnumerator Pattern4()
    {
        animator.SetTrigger("Pattern4");
        yield return new WaitUntil(() => IsAnimationClipPlaying("Wait", patternIdx + 1) == true);
    }

    private IEnumerator DeathCor()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(3f);
        float elapsedTime = 0;
        while(elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Color", new Color(1, 1, 1, 1f - elapsedTime));
        }
        Destroy(gameObject);
    }

    protected override void OnEntityDied()
    {
        statusEffects.Add(new Stun(1, 5f, this));
        nav.isStopped = true;
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(DeathCor());
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        
    }

    protected override void OnFinishPattern(int nowPatternIdx)
    {
        switch (patternIdx)
        {
            case 0:
                float rand = Random.Range(0f, 1f);
                if (rand < 0.5f)
                    patternIdx = 1;
                else
                    patternIdx = 2;
                break;
            case 1:
                patternIdx = 3;
                break;
            case 2:
                patternIdx = 3;
                break;
            case 3:
                patternIdx = 0;
                break;
            default:
                break;
        }
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
        EarthShatterEffect effect = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.EarthShatter, transform.position).GetComponent<EarthShatterEffect>();
        effect.transform.eulerAngles = transform.eulerAngles;

        Collider[] col = Physics.OverlapBox(patternTr[1].position, pattern2Box / 2, patternTr[1].rotation, 1 << LayerMask.NameToLayer("PLAYER"));
        cameraShakeSource.GenerateImpulse();
        if (col.Length > 0)
        {
            col[0].GetComponent<Entity>().TakeDamage(this, stat.Get(StatType.DAMAGE));
        }
    }

    public void Pattern3Attack()
    {
        EarthShatterEffect effect = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.EarthShatter, transform.position).GetComponent<EarthShatterEffect>();
        effect.SetStartSpeed(20f);
        effect.transform.eulerAngles = transform.eulerAngles;
        Collider[] col = Physics.OverlapBox(patternTr[2].position, pattern3Box / 2, patternTr[2].rotation, 1 << LayerMask.NameToLayer("PLAYER"));
        cameraShakeSource.GenerateImpulse();
        if (col.Length > 0)
        {
            col[0].GetComponent<Entity>().TakeDamage(this, stat.Get(StatType.DAMAGE));
        }
    }

    public void Pattern4Attack()
    {

        HitEffect effect = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.HitParticle, patternTr[3].position).GetComponent<HitEffect>();
        Collider[] col = Physics.OverlapSphere(patternTr[3].position, pattern4Rad, 1 << LayerMask.NameToLayer("PLAYER"));
        cameraShakeSource.GenerateImpulse();
        if (col.Length > 0)
        {
            col[0].GetComponent<Entity>().TakeDamage(this, stat.Get(StatType.DAMAGE));
        }
    }



    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(patternTr[0].position, patternTr[0].rotation, transform.lossyScale));
            Gizmos.color = Color.green;
            switch (patternIdx)
            {
                case 0:
                    Gizmos.DrawWireSphere(patternTr[0].position, pattern1Rad);
                    break;
                case 1:
                    Gizmos.DrawWireCube(patternTr[1].position, pattern2Box);
                    break;
                case 2:
                    Gizmos.DrawWireCube(patternTr[2].position, pattern3Box);
                    break;
                case 3:
                    Gizmos.DrawWireSphere(patternTr[3].position, pattern4Rad);
                    break;
                default:
                    break;
            }
            GL.PopMatrix();
        }
    }
}
