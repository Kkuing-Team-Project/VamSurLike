using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonField : Augmentation
{
    public float DAMAGE = 50f;

    int stop;



    public PoisonField(int level, AugmentationEventType eventType) : base(level, eventType)
    {
    }

   

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        CoroutineHandler.StartCoroutine(FieldAttack(e.target));
    }    
    private IEnumerator FieldAttack(Entity player)
    {
        WaitForSeconds waitTime = new WaitForSeconds(2);
        while (true)
        {
            float radius = 10;
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

            // 스킬의 지속시간을 체크할 타이머
            float durationTimer = 0;

            // 스킬의 대미지 딜레이를 체크할 타이머
            float delayTimer = 0;
            
            while (true)
            {
                durationTimer += Time.deltaTime;
                delayTimer += Time.deltaTime;
                
                if (durationTimer < 5f)
                {
                    if (delayTimer > 1f)
                    {
                        Collider[] enemies = Physics.OverlapSphere(player.transform.position, 5, 1 << LayerMask.NameToLayer("ENEMY"));
                        if (enemies.Length > 0)
                        {
                            foreach (var enemy in enemies)
                            {
                                enemy.GetComponent<Entity>().TakeDamage(player, player.stat.Get(StatType.DAMAGE));
                            }
                        }
                        delayTimer = 0f;
                    }
                } 
                else
                {
                    break;
                }

                Debug.Log($"스킬 사용 중... 스킬 사용 시간 : {durationTimer}");
                yield return null;
            }

            Debug.Log("스킬 종료");
        }
    }

}
