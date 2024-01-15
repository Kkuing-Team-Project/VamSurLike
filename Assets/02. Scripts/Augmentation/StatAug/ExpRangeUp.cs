using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpRangeUp : Augmentation
{
    public ExpRangeUp(int level, int maxLevel) : base(level, maxLevel)
    {
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_UPDATE;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        e.target.stat.Add(StatType.EXP_RANGE, 8 + level);
    }
}
