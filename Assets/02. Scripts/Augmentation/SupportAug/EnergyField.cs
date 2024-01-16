using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class EnergyField : Augmentation
{
    private Coroutine cor = null;
    public EnergyField(int level, int maxLevel) : base(level, maxLevel)
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
        }
        cor = CoroutineHandler.StartCoroutine(FieldAttack(e.target));
    }


    private IEnumerator FieldAttack(Entity player)
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.5f);
        while (true)
        {
            Collider[] enemies = Physics.OverlapSphere(player.transform.position, float.Parse(GameManager.instance.augTable[level]["EnergyField"].ToString()), 1 << LayerMask.NameToLayer("ENEMY") | 1 << LayerMask.NameToLayer("BOSS"));
            if (enemies.Length > 0)
            {
                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<Entity>().TakeDamage(player, player.stat.Get(StatType.DAMAGE) / 0.5f);
                }
            }
            yield return waitTime;
        }
    }
}
