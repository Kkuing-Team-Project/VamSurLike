using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUp : Augmentation
{
    public HPUp(int level, int maxLevel) : base(level, maxLevel)
    {
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_START;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        e.target.stat.SetDefault(StatType.MAX_HP, e.target.stat.Get(StatType.MAX_HP) + 1);
        e.target.Heal(e.target.stat.Get(StatType.MAX_HP));
    }
}
