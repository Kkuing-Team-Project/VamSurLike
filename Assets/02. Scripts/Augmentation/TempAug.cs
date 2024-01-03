using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAug : Augmentation
{
    public TempAug(PlayableCtrl player) : base(player)
    {
    }

    public override void AugmentationEffect(Entity sender, EventArgs e)
    {
        Debug.Log("임시 증강");
    }
}
