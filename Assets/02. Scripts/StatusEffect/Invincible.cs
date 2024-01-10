using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible : StatusEffect
{
    public Invincible(int level, float duration, Entity caster = null) : base(level, duration, caster)
    {
    }

    public override void OnFinish(Entity target)
    {
    }

    public override void OnStart(Entity target)
    {
    }

    public override void OnUpdate(Entity target)
    {
    }
}
