using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireBall : MonoBehaviour, IPoolable
{
    public ObjectPool pool { get; set; }

    VisualEffect effect;
    Collider coll;
    // 해당 이펙트를 사용하는 엔티티
    Entity user;

    public void OnCreate()
    {
        effect = GetComponent<VisualEffect>();
        coll = GetComponent<Collider>();
        user = GameManager.instance.player;
        coll.enabled = false;
    }

    public void OnActivate()
    {
        effect.Play();
        effect.playRate = 5f;
        coll.enabled = true;
    }

    public void ReturnObject()
    {
        pool.ReturnObject(gameObject, ObjectPool.ObjectType.FireBall);
    }

    public void StopEffect()
    {
        effect.Stop();
        effect.playRate = 10f;
        coll.enabled = false;

        StartCoroutine(CheckEffectPlaying());
    }

    IEnumerator CheckEffectPlaying()
    {
        while (effect.aliveParticleCount > 0)
        {
            yield return null;
        }
        ReturnObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENEMY"))
        {
            other.GetComponent<Entity>().TakeDamage(user, 3f);
        }
    }
}
