using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempExperienceGem : MonoBehaviour, IPoolable
{
    [Header("»πµÊ Ω√ ¿Ãµø º”µµ"), SerializeField]
    float moveSpeed;
        
    public Stack<GameObject> pool { get; set; }

    Collider coll;

    private void Start()
    {
        coll = GetComponentInChildren<Collider>();
    }

    public void Create(Stack<GameObject> pool)
    {
        this.pool = pool;
        coll = GetComponentInChildren<Collider>();
    }

    public void Push()
    {
        gameObject.SetActive(false);
        pool.Push(gameObject);
    }

    public void PullToPlayer(PlayableCtrl player)
    {
        coll.enabled = false;
        StartCoroutine(PullToPlayerCor(player));
    }

    IEnumerator PullToPlayerCor(PlayableCtrl player)
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

            if ((transform.position - player.transform.position).sqrMagnitude < 0.01f)
            {
                player.AddExp(1f);
                //Push();
                break;
            }
            yield return null;
        }
        gameObject.SetActive(false);
        yield return null;
    }
}
