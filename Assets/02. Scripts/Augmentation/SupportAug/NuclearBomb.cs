using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBomb : Augmentation
{
    private float skillTime;
    private float lastSkillUseTime;
    private int currentLevel;

    public NuclearBomb(int level, int maxLevel) : base(level, maxLevel)
    {
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_UPDATE;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        if (lastSkillUseTime + skillTime <= Time.time)
        {
            EffectAction(sender);
        }

        if (currentLevel != level)
        {
            ChangeLevel();
            EffectAction(sender);
        }
    }

    private void EffectAction(Entity player)
    {
        lastSkillUseTime = Time.time;

        ObjectPoolManager.Instance.objectPool.GetObject(
            ObjectPool.ObjectType.NuclearBomb,
            GameManager.instance.player.transform.position);
            
        foreach (var enemy in UnityEngine.Object.FindObjectsOfType<EnemyCtrl>())
        {
            enemy.GetComponent<Entity>().TakeDamage(player, enemy.hp);
        }
    }
    
    private void ChangeLevel()
    {
        currentLevel = level;
        switch (level)
        {
            case 1:
                skillTime = 240f;
                break;
            case 2:
                skillTime = 210f;
                break;
            case 3:
                skillTime = 180f;
                break;
            case 4:
                skillTime = 150f;
                break;
            case 5:
                skillTime = 120f;
                break;
        }
    }
}