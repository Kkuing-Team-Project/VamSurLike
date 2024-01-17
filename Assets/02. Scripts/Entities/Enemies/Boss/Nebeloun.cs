using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nebeloun : BossCtrl
{
    protected override void InitEntity()
    {
        base.InitEntity();
        RegisterPatterns(new PatternDelegate(Pattern1), new PatternDelegate(Pattern2), new PatternDelegate(Pattern3), new PatternDelegate(Pattern4), new PatternDelegate(Pattern5));
    }


    protected override void OnEntityDied()
    {
    }

    protected override void OnFinishPattern(int nowPatternIdx)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
    }

    private IEnumerator Pattern1()
    {
        yield return null;
    }
    private IEnumerator Pattern2()
    {

        yield return null;
    }
    private IEnumerator Pattern3()
    {

        yield return null;
    }
    private IEnumerator Pattern4()
    {

        yield return null;
    }
    private IEnumerator Pattern5()
    {

        yield return null;
    }
}
