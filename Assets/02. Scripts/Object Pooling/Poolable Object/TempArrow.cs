using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempArrow : MonoBehaviour, IPoolable
{
    public Rigidbody rigid { get; set; }
    public Stack<GameObject> pool { get; set; }

    [HideInInspector]
    public DarkArcher  archer;
    public float attackPower = 1f;  // Attack power value

    // Called when the bullet is created. Initializes the Rigidbody and sets the pool.
    public void Create(Stack<GameObject> pool)
    {
        this.pool = pool;
        rigid = GetComponent<Rigidbody>();
    }

    // Called when the bullet is enabled. Starts a coroutine to return the bullet to the pool after a set time.
    private void OnEnable()
    {
        StartCoroutine(ReturnBullet(3f));
    }

    /// <summary>
    /// Coroutine to return the bullet to the pool after the specified time.
    /// </summary>
    /// <param name="time">Time in seconds before returning the bullet</param>
    /// <returns></returns>
    private IEnumerator ReturnBullet(float time)
    {
        yield return new WaitForSeconds(time);
        Push();
    }

    // Called when the bullet collides with another object.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PLAYER"))
        {
            // If the bullet hits an enemy, apply damage and return the bullet to the pool.
            Entity playable = other.GetComponent<Entity>();
            // archer.InvokeEvent(AugmentationEventType.ON_HIT, archer, new AugEventArgs(other.transform, playable));
            playable.TakeDamage(archer, attackPower);
            Push();
        }
    }

    // Deactivates the bullet and returns it to the pool.
    public void Push()
    {
        gameObject.SetActive(false);
        pool.Push(gameObject);
    }
}
