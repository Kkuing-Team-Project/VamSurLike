using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAug : Augmentation
{
    public TempAug(int level, int maxLevel, AugmentationEventType eventType) : base(level, maxLevel, eventType)
    {
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        Debug.Log(e.eventTr);
    }
}
    