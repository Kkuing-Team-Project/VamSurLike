using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossTemp : BossController
{

    protected override void InitEntity()
    {
        base.InitEntity();
        RegisterPatterns(new PatternDelegate(Pattern1), new PatternDelegate(Pattern2));
    }

    public IEnumerator Pattern1()
    {
        animator.SetTrigger("Pattern1");
        yield return new WaitUntil(() => IsAnimationClipPlaying("Start", nowPatternIdx + 1) == true);
        yield return new WaitUntil(() => IsAnimationClipPlaying("Wait", nowPatternIdx + 1) == true);
    }

    public IEnumerator Pattern2()
    {
        animator.SetTrigger("Pattern2");
        yield return new WaitUntil(() => IsAnimationClipPlaying("Start", nowPatternIdx + 1) == true);
        yield return new WaitUntil(() => IsAnimationClipPlaying("Wait", nowPatternIdx + 1) == true);
    }

    protected override void OnEntityDied()
    {
        Debug.Log("보스 죽음");
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        
    }
}
