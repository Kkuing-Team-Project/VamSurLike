using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParticle : MonoBehaviour, IPoolable
{
    public Stack<GameObject> pool { get; set; }
    protected ParticleSystem mainParticle;
    public virtual void Create(Stack<GameObject> pool)
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
                Push();
                break;
            }
            yield return null;
        }
    }

    public virtual void Push()
    {
        gameObject.SetActive(false);
        pool.Push(gameObject);
    }
}
