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
        Enemy,
        Bullet
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

    // Number of objects to preallocate
    [SerializeField]
    private int allocateCount;

    private void Awake()
    {
        // Initialize each pool
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
                return;
            }
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                poolDictionary[pool.type].Push(obj);
            }
        }
    }

    // Method to get an object from the pool
    public GameObject Pop(ObjectType objectType)
    {
        // If the pool does not exist, return null
        if (!poolDictionary.ContainsKey(objectType))
        {
            return null;
        }

        GameObject obj;
        // Try to pop an object from the stack
        if (poolDictionary[objectType].TryPop(out obj))
        {
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // If the stack is empty, allocate more objects and return one
            Allocate(5, objectType);
            return poolDictionary[objectType].Pop();
        }
    }

    // Method to return an object back to the pool
    public void Push(GameObject obj, ObjectType type)
    {
        obj.SetActive(false);
        poolDictionary[type].Push(obj);
    }
}
