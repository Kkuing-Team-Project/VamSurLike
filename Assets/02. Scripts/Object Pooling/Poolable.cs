using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀링이 가능한 오브젝트
/// </summary>
public class Poolable : MonoBehaviour
{
    // 할당될 오브젝트 풀
    protected ObjectPool pool;

    // 생성시 호출할 메서드
    public virtual void Create(ObjectPool pool)
    {
        this.pool = pool;
        gameObject.SetActive(false);
    }

    // 오브젝트 풀에 Push
    public virtual void Push()
    {
        pool.Push(this);
    }
}
