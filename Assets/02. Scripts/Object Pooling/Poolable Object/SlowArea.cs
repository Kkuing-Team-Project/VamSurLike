using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowArea : MonoBehaviour, IPoolable
{
    public ObjectPool pool { get; set; }

    List<Entity> targets = new List<Entity>();
    public void OnCreate()
    {
    }

    public void OnActivate()
    {
        targets.Clear();
    }

    public void ReturnObject()
    {
        pool.ReturnObject(gameObject, ObjectPool.ObjectType.SlowArea);
    }

    public void StartEffect(float areaRange, float duration, int slowPercent, List<Entity> targets)
    {
        StartCoroutine(CheckTargets(areaRange, duration, slowPercent, targets));
    }

    IEnumerator CheckTargets(float areaRange, float duration, int slowPercent, List<Entity> targets)
    {
        float durationTimer = 0f;
        float checkTick = 0.1f;

        while(durationTimer < duration)
        {
            Collider[] entities = Physics.OverlapSphere(transform.position, areaRange, 1 << LayerMask.NameToLayer("ENEMY") | 1 << LayerMask.NameToLayer("BOSS"));
            if (entities.Length > 0)
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity target = entities[i].GetComponent<Entity>();
                    // 타깃이 효과를 받았던 객체라면 무시
                    if (targets.Contains(target))
                    {
                        Debug.Log("무시");
                        continue;
                    }
                    Debug.Log("효과 적용");

                    // 아니라면 효과 적용
                    target.AddEffect(new Slow(targets, slowPercent, 0.5f, target));
                    targets.Add(target);
                }
            }
            durationTimer += checkTick;
            yield return new WaitForSeconds(checkTick);
        }

        ReturnObject();
    }
}
