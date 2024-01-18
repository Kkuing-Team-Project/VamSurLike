using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slow Effect
/// level: 슬로우 수치(%)
/// </summary>
public class Slow : StatusEffect
{
    public Slow(int level, float duration, Entity caster = null) : base(level, duration, caster)
    {
    }

    public override void OnFinish(Entity target)
    {
        target.stat.Multiply(StatType.MOVE_SPEED, 1 / (1f - (Mathf.Clamp(level, 0, 100) / 100f)));
    }

    public override void OnStart(Entity target)
    {
        target.stat.Multiply(StatType.MOVE_SPEED, (1f - (Mathf.Clamp(level, 0, 100) / 100f)));
    }

    public override void OnUpdate(Entity target)
    {
        //target.stat.Multiply(StatType.MOVE_SPEED, 1f - (Mathf.Clamp(level, 0, 100) / 100f));
    }
}
