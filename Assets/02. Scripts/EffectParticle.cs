using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParticle : MonoBehaviour, IPoolable
{
    public ObjectPool pool { get; set; }

    public float effectSize = 1f;

    protected ParticleSystem mainParticle;
    protected ObjectPool.ObjectType objectType;
    public virtual void OnCreate()
    {
        mainParticle = transform.GetComponent<ParticleSystem>();
    }
    public virtual void OnActivate()
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
        pool.ReturnObject(gameObject, objectType);
    }

    public void StopEffect()
    {
        mainParticle?.Stop();
    }
}
