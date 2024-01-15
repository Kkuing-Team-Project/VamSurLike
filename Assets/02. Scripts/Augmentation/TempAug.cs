using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAug : Augmentation
{
    public TempAug(int level, int maxLevel) : base(level, maxLevel)
    {
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_UPDATE;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        Debug.Log(e.eventTr);
    }
}
    