using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour, IPoolable
{
    public Stack<GameObject> pool { get; set; }
    ParticleSystem particle;
    public void Create(Stack<GameObject> pool)
    {
        this.pool = pool;
        particle = transform.GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        if (particle != null)
        {
            particle.Play();
            StartCoroutine(CheckPlaying());
        }        
    }

    IEnumerator CheckPlaying()
    {
        while (true)
        {
            if (!particle.isPlaying)
            {
                Push();
                break;
            }
            yield return null;
        }
    }

    public void Push()
    {
        gameObject.SetActive(false);
        pool.Push(gameObject);
    }
}
