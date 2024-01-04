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

    IEnumerator ReturnBullet(float time)
    {
        yield return new WaitForSeconds(time);
        Push();
    }
}
