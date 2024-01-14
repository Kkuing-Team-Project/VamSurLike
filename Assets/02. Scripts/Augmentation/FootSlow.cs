using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using JetBrains.Annotations;

public class FootSlow : Augmentation
{
    private float lastActionTime;
    private float delay;

    private float duration;
    private float speedDownPercent = 0.2f;

    private HashSet<Entity> changedEntities = new HashSet<Entity>();

    public FootSlow(int level, int maxLevel, AugmentationEventType eventType) : base(level, maxLevel, eventType)
    {
        lastActionTime = Time.time;
        delay = 0f;
        duration = 1.0f;
    }


    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        if (lastActionTime + delay > Time.time)
            return;
        lastActionTime = Time.time;

        switch (level)
        {
            case 1:
                speedDownPercent = 0.2f;
                break;
            case 2:
                speedDownPercent = 0.4f;
                break;
            case 3:
                speedDownPercent = 0.6f;
                break;
            case 4:
                speedDownPercent = 0.8f;
                break;
            case 5:
                speedDownPercent = 1.0f;
                break;
            default:
                break;
        }

        CoroutineHandler.StartCoroutine(Action(sender));
    }

    private IEnumerator Action(Entity sender)
    {
        float radius = 5;
        float delayTimer = 0;
        Vector3 pos = sender.transform.position;
        
        for (float durationTimer = 0; durationTimer < duration; durationTimer += Time.deltaTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            
            delayTimer += Time.deltaTime;
            if (delayTimer >= 1)
            {
                delayTimer -= 1;
                pos = sender.transform.position;
                foreach (var entity in changedEntities)
                {
                    entity.stat.Add(StatType.MOVE_SPEED, entity.stat.Get(StatType.MOVE_SPEED) * speedDownPercent); 
                    entity.stat.UpdateStat();
                }
                changedEntities.Clear();
            }
            
            Collider[] colliders;
            colliders = Physics.OverlapSphere(pos, radius, 1 << LayerMask.NameToLayer("ENEMY"));
            foreach (Collider collider in colliders)
            {
                Entity entity = collider.GetComponent<Entity>();
                if (entity)
                {
                    if (!changedEntities.Contains(entity))
                    {
                        Stat eneityStat = entity.stat;
                        changedEntities.Add(entity);
                        eneityStat.Add(StatType.MOVE_SPEED, -(eneityStat.Get(StatType.MOVE_SPEED) * speedDownPercent));
                        eneityStat.UpdateStat();
                    }
                }
            }
        }
    }
}