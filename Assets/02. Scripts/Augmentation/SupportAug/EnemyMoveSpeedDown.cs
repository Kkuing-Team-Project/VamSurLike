using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveSpeedDown : Augmentation
{
    public EnemyMoveSpeedDown(int level, int maxLevel) : base(level, maxLevel)
    {
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_UPDATE;
    }
}
