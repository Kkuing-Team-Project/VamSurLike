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
    /// 현재 오브젝트를 time 초 후에 오브젝트 풀로 반환합니다.
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
            TempEnemy enemy = other.GetComponent<TempEnemy>();
            enemy.TakeDamage(10f);
            StopAllCoroutines();
            Push();
        }
    }
}
