using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

public class FootSlow : Augmentation
{




    public FootSlow(int level, int maxLevel, AugmentationEventType eventType) : base(level, maxLevel, eventType)
    {
    }


    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        CoroutineHandler.StartCoroutine(FieldAttack(e.target));
    }
    public float slowLength = 4f;

    private IEnumerator FieldAttack(Entity player)
    {
       

        while (true)
        {
            float Speed = 0.2f;
            switch (level)
            {
                case 1:
                    Speed = 0.2f;
                    break;
                case 2:
                    Speed = 0.3f;
                    break;
                case 3:
                    Speed = 0.4f;
                    break;
                case 4:
                    Speed = 0.5f;
                    break;
                case 5:
                    Speed = 0.6f;
                    break;
                default:
                    break;
            }
            
            Time.timeScale = Speed;
            Time.fixedDeltaTime = Time.timeScale * 0.2f;
        }
       
              
    }
}
