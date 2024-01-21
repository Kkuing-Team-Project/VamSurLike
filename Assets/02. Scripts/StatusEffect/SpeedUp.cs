using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : StatusEffect
{
    public SpeedUp(int level, float duration, Entity caster = null) : base(level, duration, caster)
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
        target.stat.Multiply(StatType.MOVE_SPEED, (float)level / 100f);
    }
}
