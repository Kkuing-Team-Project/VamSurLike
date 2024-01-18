using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBomb : Augmentation
{
    private float skillTime;
    private float lastSkillUseTime;
    private int currentLevel;
    private Coroutine cor;

    public NuclearBomb(int level, int maxLevel) : base(level, maxLevel)
    {
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_START;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        if(cor != null)
        {
            CoroutineHandler.StopCoroutine(cor);
            cor = null;
        }

        cor = CoroutineHandler.StartCoroutine(EffectActionCor(sender));
    }

    private IEnumerator EffectActionCor(Entity player)
    {
        while (true)
        {
            foreach (var enemy in UnityEngine.Object.FindObjectsOfType<EnemyCtrl>())
            {
                enemy.GetComponent<Entity>().TakeDamage(player, enemy.hp);
            }

            ObjectPoolManager.Instance.objectPool.GetObject(
                ObjectPool.ObjectType.NuclearBomb,
                GameManager.instance.player.transform.position);

            // Debug.Log(GameManager.instance.augTable[level]["NuclearBomb"].ToString());
            yield return new WaitForSeconds(float.Parse(GameManager.instance.augTable[level]["NuclearBomb"].ToString()));
        }
    }
}