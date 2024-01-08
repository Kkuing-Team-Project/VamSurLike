using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempBullet : MonoBehaviour, IPoolable
{
    public Rigidbody rigid { get; set; }
    public Stack<GameObject> pool { get; set; }

    TempPlayable player;

    public void Create(Stack<GameObject> pool)
    {
        this.pool = pool;
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(ReturnBullet(3f));
    }

    /// <summary>
    /// ���� ������Ʈ�� time �� �Ŀ� ������Ʈ Ǯ�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator ReturnBullet(float time)
    {
        yield return new WaitForSeconds(time);
        Push();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENEMY"))
        {
            Entity enemy = other.GetComponent<Entity>();
            enemy.TakeDamage(player, 10f);
            StopAllCoroutines();
            Push();
        }
    }

    public void Push()
    {
        gameObject.SetActive(false);
        pool.Push(gameObject);
    }
}
