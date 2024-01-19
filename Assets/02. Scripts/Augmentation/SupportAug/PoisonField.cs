using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonField : Augmentation
{
    private Coroutine cor;

    public PoisonField(int level, int maxLevel) : base(level, maxLevel)
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
        float damageDelayTimer = 0;
        float radius = 0f;
        Vector3 dmagePosition = player.transform.position;
        while (true)
        {
            radius = float.Parse(GameManager.instance.augTable[level]["PoisonField"].ToString());

            yield return new WaitForSeconds(5);
            dmagePosition = player.transform.position;

            PoisonFieldEffect effect = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.PoisonField, dmagePosition).GetComponent<PoisonFieldEffect>();
            effect.SetEffectSize(radius);
            effect.SetDuration(5f);

            damageDelayTimer = 0;
            
            while (true)
            {
                damageDelayTimer += Time.deltaTime;                

                if(damageDelayTimer >= 0.25f)
                {                    
                    Collider[] enemies = Physics.OverlapSphere(dmagePosition, radius, 1 << LayerMask.NameToLayer("ENEMY") | 1 << LayerMask.NameToLayer("BOSS"));
                    if (enemies.Length > 0)
                    {
                        foreach (var enemy in enemies)
                        {
                            enemy.GetComponent<Entity>().TakeDamage(player, player.stat.Get(StatType.DAMAGE) * 0.25f);
                        }
                    }
                    damageDelayTimer = 0f;
                }
                yield return null;
            }
        }
    }
}