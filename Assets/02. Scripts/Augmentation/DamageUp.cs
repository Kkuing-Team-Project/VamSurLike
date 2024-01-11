using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUp : Augmentation
{
    public float radius;
    public float maintainTime;
    
    public DamageUp(int level, int maxLevel, AugmentationEventType eventType) : base(level, maxLevel, eventType)
    {
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        e.target.stat.Multiply(StatType.DAMAGE, 1.1f * level);
        Debug.Log($"플레이어 대미지:  {e.target.stat.Get(StatType.DAMAGE)}");
    }
}
