using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBullet : Poolable
{
    public Rigidbody RB { get; set; }

    public override void Create(ObjectPool pool)
    {
        base.Create(pool);
        RB = GetComponent<Rigidbody>();
    }

    public void StartReturn(float time)
    {
        StartCoroutine(ReturnBullet(time));
    }

    IEnumerator ReturnBullet(float time)
    {
        yield return new WaitForSeconds(time);
        Push();
    }
}
