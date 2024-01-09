using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAug : Augmentation
{
    public TempAug(int level, AugmentationEventType eventType) : base(level, eventType)
    {
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        Debug.Log(e.eventTr);
    }
}
    