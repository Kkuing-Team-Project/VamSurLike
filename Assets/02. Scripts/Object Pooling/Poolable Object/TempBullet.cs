using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempBullet : MonoBehaviour, IPoolable
{
    public Rigidbody rigid { get; set; }
    public Stack<GameObject> pool { get; set; }

    [HideInInspector]
    public PlayableCtrl player;

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
        if (other.gameObject.CompareTag("ENEMY"))
        {
            // If the bullet hits an enemy, apply damage and return the bullet to the pool.
            Entity enemy = other.GetComponent<Entity>();
            player.InvokeEvent(AugmentationEventType.ON_HIT, player, new AugEventArgs(other.transform, enemy));
            enemy.TakeDamage(player, 10f);
            StopAllCoroutines();
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
