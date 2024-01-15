using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedUp : Augmentation
{
    public MoveSpeedUp(int level, int maxLevel) : base(level, maxLevel)
    {
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_UPDATE;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        e.target.stat.Add(StatType.MOVE_SPEED, 6 + level);
    }
}
