using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSlow : Augmentation
{
    private float augmentationDelay = 10;
    List<Entity> targets = new List<Entity>();
    Coroutine actionCoroutine;
    

    public FootSlow(int level, int maxLevel) : base(level, maxLevel)
    {
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_START;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        // 슬로우 퍼센트
        augmentationDelay = float.Parse(GameManager.instance.augTable[level]["FootSlow"].ToString());
        if (actionCoroutine != null)
        {
            CoroutineHandler.StopCoroutine(actionCoroutine);
        }
        actionCoroutine = CoroutineHandler.StartCoroutine(Action(e.target));
    }


    private IEnumerator Action(Entity sender)
    {
        float delayTimer = 0f;
        while (true)
        {
            for (float durationTimer = 0; durationTimer < 5f; durationTimer += Time.deltaTime)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                delayTimer += Time.deltaTime;
                if (delayTimer >= 0.5f)
                {
                    delayTimer = 0f;
                    SlowArea newArea = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.SlowArea, sender.transform.position).GetComponent<SlowArea>();
                    newArea.StartEffect(2f, 2f, 50, targets);
                }
            }
            yield return new WaitForSeconds(augmentationDelay);
        }
    }
}