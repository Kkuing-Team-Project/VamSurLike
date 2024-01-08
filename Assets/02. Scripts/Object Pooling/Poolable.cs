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

    /// <summary>
    /// 풀링할 오브젝트를 새로 생성할 때 실행할 메서드
    /// </summary>
    /// <param name="pool">할당될 풀 오브젝트</param>
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
