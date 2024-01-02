using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public int level;
    public float duration;
    public Entity caster { get; private set; }
    public float maxDuration { get; private set; }

    public StatusEffect(int level, float duration, Entity caster = null)
    {
        this.level = level;
        maxDuration = this.duration = duration;
        this.caster = caster;
    }

    public abstract void OnStart(Entity target);
    public abstract void OnUpdate(Entity target);
    public abstract void OnFinish(Entity target);
}