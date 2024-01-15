using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExperienceGem : MonoBehaviour, IPoolable
{
    [Header("ȹ�� �� �̵� �ӵ�"), SerializeField]
    float moveSpeed;
        
    public Queue<GameObject> pool { get; set; }

    Collider coll;

    private void Start()
    {
        coll = GetComponent<Collider>();
    }

    public void Create(Queue<GameObject> pool)
    {
        this.pool = pool;
        coll = GetComponentInChildren<Collider>();
    }

    public void ReturnObject()
    {
        gameObject.SetActive(false);
        coll.enabled = true;
        pool.Enqueue(gameObject);
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
                break;
            }
            yield return null;
        }
        ReturnObject();
        yield return null;
    }

}
