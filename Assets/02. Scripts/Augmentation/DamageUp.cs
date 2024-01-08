using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUp : Augmentation
{
    public float radius;
    public float maintainTime;
    
    public DamageUp(PlayableCtrl player, int level, AugmentationEventType eventType) : base(player, level, eventType)
    {
    }

    public override void AugmentationEffect(Entity sender, EventArgs e)
    {
        player.stat.Add(StatType.DAMAGE, player.stat.Get(StatType.DAMAGE) * 0.1f * level);
    }
}
