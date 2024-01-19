using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempArrow : MonoBehaviour, IPoolable
{
    public Rigidbody rigid { get; set; }
    public ObjectPool pool { get; set; }

    private DarkArcher archer;
    private Entity target;
    private float attackPower = 1f;  // Attack power value
    public void OnCreate()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void OnActivate()
    {
        StartCoroutine(ReturnBullet(3f));
    }
    
    public void SetTarget(DarkArcher archer, Entity target)
    {
        this.archer = archer;
        this.target = target;
    }


    /// <summary>
    /// </summary>
    /// <param name="time">Time in seconds before returning the bullet</param>
    /// <returns></returns>
    private IEnumerator ReturnBullet(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity entity) && entity == target)
        {
            entity.TakeDamage(archer, attackPower);
            StopAllCoroutines();
            ReturnObject();
        }
    }

    public void ReturnObject()
    {
        pool.ReturnObject(gameObject, ObjectPool.ObjectType.Arrow);
    }

}
