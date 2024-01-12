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
        e.target.stat.Add(StatType.DAMAGE, float.Parse(GameManager.instance.augTable[level]["C_damage up"].ToString()));
    }
}
