using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUp : Augmentation
{
    public DamageUp(int level, int maxLevel) : base(level, maxLevel)
    {
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        e.target.stat.Add(StatType.DAMAGE, float.Parse(GameManager.instance.augTable[level]["DamageUp"].ToString()));
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_UPDATE;
    }
}
