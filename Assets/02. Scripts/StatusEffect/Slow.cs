using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

/// <summary>
/// Slow Effect
/// level: 슬로우 수치(%)
/// </summary>
public class Slow : StatusEffect
{
    List<Entity> targetList;
    public Slow(List<Entity> targetList, int level, float duration, Entity caster = null) : base(level, duration, caster)
    {
        this.targetList = targetList;
    }

    public override void OnStart(Entity target)
    {
        for (int i = 0; i < target.meshRenderer.materials.Length; i++)
        {
            target.meshRenderer.materials[i].color = new Color(0.9f, 0.6f, 0.7f);
        }
    }

    public override void OnFinish(Entity target)
    {
        targetList.Remove(target);

        for (int i = 0; i < target.meshRenderer.materials.Length; i++)
        {
            target.meshRenderer.materials[i].color = Color.white;
        }
    }

    public override void OnUpdate(Entity target)
    {
        target.stat.Multiply(StatType.MOVE_SPEED, 1f - (float)(Mathf.Clamp(level, 0, 100) / 100f));
    }
}
