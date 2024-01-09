using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUp : Augmentation
{
    public float radius;
    public float maintainTime;
    
    public DamageUp(int level, AugmentationEventType eventType) : base(level, eventType)
    {
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        e.target.stat.Add(StatType.DAMAGE, e.target.stat.Get(StatType.DAMAGE) * 0.1f * level);
    }
}
