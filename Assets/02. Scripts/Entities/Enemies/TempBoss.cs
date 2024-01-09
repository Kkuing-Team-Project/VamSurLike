using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempBoss : BossCtrl
{
    public Transform pattern1Tr;

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
        yield return new WaitUntil(() => IsAnimationClipPlaying("Start", patternIdx + 1) == true);
        yield return new WaitUntil(() => IsAnimationClipPlaying("Wait", patternIdx + 1) == true);
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

    protected override void OnEntityDied()
    {
        Debug.Log("º¸½º »ç¸Á");
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
        Collider[] col = Physics.OverlapSphere(pattern1Tr.position, 10, 1 << LayerMask.NameToLayer("PLAYER"));

        if (col.Length > 0)
        {
            col[0].GetComponent<Entity>().TakeDamage(this, stat.Get(StatType.DAMAGE));
            Debug.LogError(col[0].GetComponent<Entity>().hp);
        }
    }
}
