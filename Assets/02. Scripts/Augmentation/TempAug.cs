using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAug : Augmentation
{
    public TempAug(PlayableCtrl player, int level, AugmentationEventType eventType) : base(player, level, eventType)
    {
    }

    public override void AugmentationEffect(Entity sender, EventArgs e)
    {
        Debug.Log((e as OnHitArgs).hitPoint);
    }
}
