using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempBullet : Poolable
{
    public Rigidbody rigid { get; set; }

    public override void Create(ObjectPool pool)
    {
        base.Create(pool);
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
            enemy.TakeDamage(null, 10f);
            StopAllCoroutines();
            Push();
        }
    }
}
