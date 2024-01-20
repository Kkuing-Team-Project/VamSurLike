using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExperienceGem : MonoBehaviour, IPoolable
{
    [Header("ȹ�� �� �̵� �ӵ�"), SerializeField]
    float moveSpeed;
    public ObjectPool pool { get; set; }

    Collider coll;
    public Coroutine parabolicCor { get; private set; }
    public void OnCreate()
    {
        coll = GetComponent<Collider>();
    }

    public void OnActivate()
    {
    }

    public void ReturnObject()
    {
        coll.enabled = true;
        pool.ReturnObject(gameObject, ObjectPool.ObjectType.Experience);
    }

    public void PullToPlayer(PlayableCtrl player)
    {
        coll.enabled = false;
        StartCoroutine(PullToPlayerCor(player));
    }

    IEnumerator PullToPlayerCor(PlayableCtrl player)
    {
        Vector3 targetPosition;
        while (true)
        {
            targetPosition = player.transform.position;
            targetPosition.y = 0;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if ((transform.position - targetPosition).sqrMagnitude < 0.01f)
            {
                player.AddExp(1f);
                SoundManager.Instance.PlaySound("Sound_EF_CH_EXP02");
                break;
            }
            yield return null;
        }
        ReturnObject();
        yield return null;
    }

    public void ParabolicMovement(Vector3 position)
    {
        if (parabolicCor != null)
        {
            StopCoroutine(parabolicCor);
        }
        parabolicCor = StartCoroutine(ParabolicMove(position));
    }

    IEnumerator ParabolicMove(Vector3 position)
    {
        while (true)
        {
            if ((position - transform.position).sqrMagnitude <= 0.01f)
            {
                break;
            }
            transform.position = Vector3.Slerp(transform.position, position, 2.5f * Time.deltaTime);
            yield return null;
        }
        parabolicCor = null;
    }
}
