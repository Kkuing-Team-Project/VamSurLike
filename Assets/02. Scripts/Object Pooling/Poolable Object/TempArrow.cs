using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempArrow : MonoBehaviour, IPoolable
{
    public Rigidbody rigid { get; set; }
    public Queue<GameObject> pool { get; set; }

    [HideInInspector]
    private DarkArcher  archer;
    private float attackPower = 1f;  // Attack power value

    public void Create(Queue<GameObject> pool)
    {
        this.pool = pool;
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
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
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }
}
