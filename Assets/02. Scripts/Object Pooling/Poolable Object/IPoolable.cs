using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    // 할당될 오브젝트 풀
    public ObjectPool pool { get; set; }

    /// <summary>
    /// 오브젝트를 새로 생성했을 때 실행할 메서드
    /// </summary>
    public abstract void OnCreate();
    /// <summary>
    /// 풀링할 오브젝트를 활성화 했을 때 실행할 메서드
    /// </summary>
    /// <param name="pool">할당될 풀</param>
    public abstract void OnActivate();
    /// <summary>
    /// 오브젝트를 풀에 반환할 때 실행할 메서드
    /// </summary>
    public abstract void ReturnObject();
}