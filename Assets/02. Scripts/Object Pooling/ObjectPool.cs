using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        Experience,
        CollapseZone,
        HitParticle,
        PoisonField,
        DamageText,
        Meteor,
        FireBall,
        EarthShatter,
        Shield,
        MagicCircle,
        FreezeCircle,
        NuclearBomb
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
    public Dictionary<ObjectType, Queue<GameObject>> poolDictionary = new Dictionary<ObjectType, Queue<GameObject>>();
    private void Awake()
    {
        // Initialize each pool
        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            new GameObject(pool.type.ToString()).transform.SetParent(transform);
            poolDictionary.Add(pool.type, objectPool);

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);

                obj.SetActive(false);
                obj.transform.SetParent(transform.Find(pool.type.ToString()).transform);

                objectPool.Enqueue(obj);

                IPoolable poolable = obj.GetComponent<IPoolable>();
                poolable.pool = this;
                poolable.OnCreate();
            }
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
                obj.transform.SetParent(transform.Find(pool.type.ToString()).transform);
                poolDictionary[pool.type].Enqueue(obj);

                IPoolable poolable = obj.GetComponent<IPoolable>();
                poolable.pool = this;
                poolable.OnCreate();
            }
        }
    }

    // Method to get an object from the pool
    public GameObject GetObject(ObjectType objectType, Vector3 position)
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
        if (poolDictionary[objectType].TryDequeue(out obj))
        {            
            if (poolDictionary[objectType].Count < 3)
            {
                Allocate(5, objectType);
            }

            obj.transform.position = position;
            obj.SetActive(true);
            
            obj.GetComponent<IPoolable>().OnActivate();
            return obj;
        }
        else
        {
            Allocate(3, objectType);
            if (poolDictionary[objectType].Count > 0)
            {
                obj = poolDictionary[objectType].Dequeue();

                obj.transform.position = position;
                obj.SetActive(true);
                
                obj.GetComponent<IPoolable>().OnActivate();
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
    public void ReturnObject(GameObject obj, ObjectType type)
    {
        obj.SetActive(false);
        poolDictionary[type].Enqueue(obj);
    }
}
