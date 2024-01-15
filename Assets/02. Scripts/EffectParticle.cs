using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParticle : MonoBehaviour, IPoolable
{
    public Queue<GameObject> pool { get; set; }
    protected ParticleSystem mainParticle;
    public virtual void Create(Queue<GameObject> pool)
    {
        this.pool = pool;
        mainParticle = transform.GetComponent<ParticleSystem>();
    }

    protected virtual void OnEnable()
    {
        if (mainParticle != null)
        {
            mainParticle.Play();
            StartCoroutine(CheckPlaying());
        }
    }

    IEnumerator CheckPlaying()
    {
        while (true)
        {
            if (!mainParticle.isPlaying)
            {
                ReturnObject();
                break;
            }
            yield return null;
        }
    }

    public virtual void ReturnObject()
    {
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }
}
