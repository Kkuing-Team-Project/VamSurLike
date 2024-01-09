using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public enum ObjectType
    {
        Enemy,
        Bullet
    }

    [System.Serializable]
    public struct Pool 
    {
        public ObjectType type;     // 오브젝트 타입
        public GameObject prefab;   // 오브젝트 프리펩
        public int size;            // 초기 풀 사이즈
    }

    // 풀 리스트
    public List<Pool> pools;

    public Dictionary<ObjectType, Stack<GameObject>> poolDictionary = new Dictionary<ObjectType, Stack<GameObject>>();


    private void Awake()
    {
        foreach (var pool in pools)
        {
            Stack<GameObject> objectPool = new Stack<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Push(obj);
                obj.GetComponent<IPoolable>().Create(objectPool);
            }
            poolDictionary.Add(pool.type, objectPool);
        }
    }

    /// <summary>
    /// 할당
    /// </summary>
    /// <param name="allocateCount">할당 개수</param>
    public void Allocate(int count, ObjectType objectType)
    {
        foreach(var pool in pools)
        {
            if (pool.type != objectType)
            {
                continue;
            }
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                poolDictionary[pool.type].Push(obj);
                obj.GetComponent<IPoolable>().Create(poolDictionary[pool.type]);

            }
        }
    }

    public GameObject Pop(ObjectType objectType)
    {
        if (!poolDictionary.ContainsKey(objectType))
        {
            // 없다면 새로운 풀 만들기 추가할 것.
            return null;
        }

        GameObject obj;
        if (poolDictionary[objectType].TryPop(out obj))
        {
            obj.SetActive(true);

            if (poolDictionary[objectType].Count < 3)
            {
                Allocate(5, objectType);
            }
            return obj;
        }
        else
        {
            Allocate(3, objectType);
            if (poolDictionary[objectType].Count > 0)
            {
                return poolDictionary[objectType].Pop();
            }
            else
            {
                return null;
            }
        }
    }


    public void Push(GameObject obj, ObjectType type)
    {
        obj.SetActive(false);
        poolDictionary[type].Push(obj);
    }
}
