using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    // 할당될 오브젝트 풀
    public Queue<GameObject> pool { get; set; }

    /// <summary>
    /// 풀링할 오브젝트를 새로 생성할 때 실행할 메서드
    /// </summary>
    /// <param name="pool">할당될 풀</param>
    public abstract void Create(Queue<GameObject> pool);
    public abstract void ReturnObject();
}