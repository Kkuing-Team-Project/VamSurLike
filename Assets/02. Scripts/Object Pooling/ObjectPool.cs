using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Enum to define different object types
    public enum ObjectType
    {
        None,
        StoneHead,
        Prion,
        Servant,
        DarkArcher,
        Bullet,
        Arrow,
        Experience
    }

    [System.Serializable]
    public struct Pool 
    {
        public ObjectType type;     // Type of object
        public GameObject prefab;   // Prefab for the object
        public int size;            // Size of the pool
    }

    // List to hold different pools
    public List<Pool> pools;

    // Dictionary to map each ObjectType to a stack of GameObjects
    public Dictionary<ObjectType, Stack<GameObject>> poolDictionary = new Dictionary<ObjectType, Stack<GameObject>>();

    private void Awake()
    {
        // Initialize each pool
        foreach (var pool in pools)
        {
            Stack<GameObject> objectPool = new Stack<GameObject>();
            new GameObject(pool.type.ToString()).transform.SetParent(transform);
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);

                objectPool.Push(obj);

                obj.GetComponent<IPoolable>().Create(objectPool);

                obj.transform.SetParent(transform.Find(pool.type.ToString()).transform);
            }
            poolDictionary.Add(pool.type, objectPool);
        }
    }


    /// <summary>
    /// Allocate additional objects
    /// </summary>
    /// <param name="count">Number of objects to allocate</param>
    public void Allocate(int count, ObjectType objectType)
    {
        // Allocate objects of a specific type
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

                obj.transform.SetParent(transform.Find(pool.type.ToString()).transform);

            }
        }
    }

    // Method to get an object from the pool
    public GameObject Pop(ObjectType objectType, Vector3 position)
    {
        if (objectType == ObjectType.None)
            return null;
        
        // If the pool does not exist, return null
        if (!poolDictionary.ContainsKey(objectType))
        {
            Debug.LogError("Pool for type " + objectType + " does not exist.");
            return null;
        }

        GameObject obj;
        // Try to pop an object from the stack
        if (poolDictionary[objectType].TryPop(out obj))
        {
            obj.transform.position = position;
            obj.SetActive(true);
            Debug.Log("Popped and activated object of type: " + objectType); // 로그 추가
            
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
                obj = poolDictionary[objectType].Pop();
                obj.transform.position = position;
                obj.SetActive(true);
                Debug.Log("Allocated and activated object of type: " + objectType); // 로그 추가
                
                return obj;
            }
            else
            {
                Debug.LogError("Failed to allocate object of type " + objectType);
                return null;
            }
        }
    }
    
    // Method to return an object back to the pool
    public void Push(GameObject obj, ObjectType type)
    {
        obj.SetActive(false);
        poolDictionary[type].Push(obj);
    }
}
