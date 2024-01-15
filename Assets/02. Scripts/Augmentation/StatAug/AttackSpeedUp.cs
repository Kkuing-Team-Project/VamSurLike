using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedUp : Augmentation
{
    public AttackSpeedUp(int level, int maxLevel) : base(level, maxLevel)
    {
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_UPDATE;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        e.target.stat.Add(StatType.ATTACK_SPEED, float.Parse(GameManager.instance.augTable[level]["AttackSpeedUp"].ToString()));
    }
}
