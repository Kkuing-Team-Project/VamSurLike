using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonField : Augmentation
{
    public float DAMAGE = 0.5f;
    

    public PoisonField(int level, AugmentationEventType eventType) : base(level, eventType)
    {
    }

   

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        CoroutineHandler.StartCoroutine(FieldAttack(e.target));
    }
    private IEnumerator FieldAttack(Entity player)
    {
        


        WaitForSeconds waitTime = new WaitForSeconds(10);
        while (true)
        {
            float radius = 0;
            switch (level)
            {
                case 1:
                    radius = 1;
                    break;
                case 2:
                    radius = 2;
                    break;
                case 3:
                    radius = 3;
                    break;
                case 4:
                    radius = 4;
                    break;
                case 5:
                    radius = 6;
                    break;
                default:
                    break;
            }
            
            yield return waitTime;
            Debug.Log("10초후 스킬 발동");

           


            Collider[] enemies = Physics.OverlapSphere(player.transform.position, radius, 1 << LayerMask.NameToLayer("ENEMY"));
            if (enemies.Length > 0 )
            {
                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<Entity>().TakeDamage(player, player.stat.Get(StatType.DAMAGE));
                }
            }
        }
        




    }

}
