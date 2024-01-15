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
        int idx = -1;
        for (int i = 0; i < GameManager.instance.statTable.Count; i++)
        {
            if (e.target.entityType.ToString().Equals(GameManager.instance.statTable[i]["CHARACTER_ID"].ToString()))
            {
                idx = i;
                break;
            }
        }
        Debug.Log(float.Parse(GameManager.instance.statTable[idx]["MAX_HP"].ToString()) + float.Parse(GameManager.instance.augTable[level]["HPUp"].ToString()));
        e.target.stat.SetDefault(StatType.MAX_HP, float.Parse(GameManager.instance.statTable[idx]["MAX_HP"].ToString()) + float.Parse(GameManager.instance.augTable[level]["HPUp"].ToString()));
        e.target.Heal(e.target.stat.Get(StatType.MAX_HP));
    }
}
