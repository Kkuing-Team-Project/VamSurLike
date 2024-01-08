using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyField : Augmentation
{
    public EnergyField(PlayableCtrl player, int level, AugmentationEventType eventType) : base(player, level, eventType)
    {
    }

    public override void AugmentationEffect(Entity sender, EventArgs e)
    {

    }
}
