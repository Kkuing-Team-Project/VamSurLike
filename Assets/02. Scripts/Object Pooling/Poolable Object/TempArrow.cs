using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempArrow : MonoBehaviour, IPoolable
{
    public Rigidbody rigid { get; set; }
    public ObjectPool pool { get; set; }

    [HideInInspector]
    private DarkArcher  archer;
    private float attackPower = 1f;  // Attack power value
    public void OnCreate()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void OnActivate()
    {
        StartCoroutine(ReturnBullet(3f));
    }
    
    public void SetArcher(DarkArcher archer)
    {
        this.archer = archer;
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
        if (other.gameObject.CompareTag("PLAYER"))
        {
            Entity playable = other.GetComponent<Entity>();
            playable.TakeDamage(archer, attackPower);
            StopAllCoroutines();
            ReturnObject();
        }
    }

    public void ReturnObject()
    {
        pool.ReturnObject(gameObject, ObjectPool.ObjectType.Arrow);
    }

}
