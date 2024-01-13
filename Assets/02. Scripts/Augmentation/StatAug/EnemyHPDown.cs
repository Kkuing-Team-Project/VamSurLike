using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPDown : Augmentation
{
    public EnemyHPDown(int level, int maxLevel) : base(level, maxLevel)
    {
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_UPDATE;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        base.AugmentationEffect(sender, e);
    }
}
