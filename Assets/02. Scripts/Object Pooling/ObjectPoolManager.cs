using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public ObjectPool objectPool { get; private set; }

    public override void Awake()
    {
        base.Awake();

        objectPool = GetComponentInChildren<ObjectPool>();
    }
}
