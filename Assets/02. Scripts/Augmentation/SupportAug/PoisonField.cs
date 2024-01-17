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
            cor = null;
        }
        cor = CoroutineHandler.StartCoroutine(FieldAttack(e.target));
    }

    private IEnumerator FieldAttack(Entity player)
    {
        while (true)
        {
            float radius = float.Parse(GameManager.instance.augTable[level]["PoisonField"].ToString());

            yield return new WaitForSeconds(10);
            Vector3 dmagePosition = player.transform.position;

            PoisonFieldEffect effect = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.PoisonField, dmagePosition).GetComponent<PoisonFieldEffect>();
            effect.SetEffectSize(radius);
            float durationTimer = 0;
            float delayTimer = 0;
            
            while (true)
            {
                durationTimer += Time.deltaTime;
                delayTimer += Time.deltaTime;
                
                if (durationTimer >= 5f)
                {
                    effect.StopEffect();
                    break;
                }

                if(delayTimer >= 1f)
                {
                    
                    Collider[] enemies = Physics.OverlapSphere(dmagePosition, radius, 1 << LayerMask.NameToLayer("ENEMY") | 1 << LayerMask.NameToLayer("BOSS"));
                    if (enemies.Length > 0)
                    {
                        foreach (var enemy in enemies)
                        {
                            enemy.GetComponent<Entity>().TakeDamage(player, player.stat.Get(StatType.DAMAGE));
                        }
                    }
                    delayTimer = 0f;
                }
                yield return null;
            }
            Debug.Log("Poison Field Stop");
        }
    }
}