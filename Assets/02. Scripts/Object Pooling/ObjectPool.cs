using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour 
{
    // 풀링할 오브젝트 프리팹
    [SerializeField]
    private Poolable poolObj;

    // 초기에 할당할 오브젝트 수
    [SerializeField]
    private int allocateCount;

    // 오브젝트를 넣어둘 스택
    private Stack<Poolable> poolStack = new Stack<Poolable>();

    private void Start()
    {
        Allocate(allocateCount);
    }

    /// <summary>
    /// 할당
    /// </summary>
    /// <param name="allocateCount">할당 개수</param>
    public void Allocate(int allocateCount)
    {
        for (int i = 0; i < allocateCount; i++)
        {
            Poolable allocateObj = Instantiate(poolObj, transform);
            allocateObj.Create(this);
            poolStack.Push(allocateObj);
        }
    }

    public GameObject Pop()
    {
        Poolable obj = poolStack.Pop();
        obj.gameObject.SetActive(true);

        // 풀에 오브젝트가 3개보다 적으면 5개 추가 할당
        if (poolStack.Count < 3)
        {
            Allocate(5);
        }
        return obj.gameObject;
    }

    public void Push(Poolable obj)
    {
        obj.gameObject.SetActive(false);
        poolStack.Push(obj);
    }
}
