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
        return AugmentationEventType.ON_UPDATE_ENEMY;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        e.target.stat.Add(StatType.MOVE_SPEED, float.Parse(GameManager.instance.augTable[level]["EnemyMoveSpeedDown"].ToString()));
    }
}
