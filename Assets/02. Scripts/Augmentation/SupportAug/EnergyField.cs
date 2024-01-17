using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class EnergyField : Augmentation
{
    private Coroutine cor = null;
    private Transform particle = null;
    private float particleDefaultSize;
    
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

        if (!particle)
        {
            particle = ObjectPoolManager.Instance.objectPool.GetObject(
                ObjectPool.ObjectType.EnergyField,
                sender.transform.position).transform;
            particle.SetParent(sender.transform);
            particleDefaultSize = particle.localScale.x;
        }
        
        cor = CoroutineHandler.StartCoroutine(FieldAttack(e.target));
    }

    private IEnumerator FieldAttack(Entity player)
    {

        WaitForSeconds waitTime = new WaitForSeconds(0.5f);
        while (true)
        {
            float radius = float.Parse(GameManager.instance.augTable[level]["EnergyField"].ToString());
            particle.localScale = Vector3.one * (particleDefaultSize * radius);
            Collider[] enemies = Physics.OverlapSphere(player.transform.position, radius, 1 << LayerMask.NameToLayer("ENEMY") | 1 << LayerMask.NameToLayer("BOSS"));
            if (enemies.Length > 0)
            {
                foreach (var enemy in enemies)
                {
                    Debug.Log(enemy.name);
                    enemy.GetComponent<Entity>().TakeDamage(player, player.stat.Get(StatType.DAMAGE) / 0.5f);
                }
            }
            yield return waitTime;
        }
    }
}
