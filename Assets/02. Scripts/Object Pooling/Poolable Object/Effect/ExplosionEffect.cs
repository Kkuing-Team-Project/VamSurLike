using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : EffectParticle
{
    public override void OnActivate()
    {
        base.OnActivate();
        objectType = ObjectPool.ObjectType.Explosion;
        transform.position = new Vector3(transform.position.x, GameManager.instance.transform.position.y, transform.position.z);
    }

    public void SetSize(Vector3 size) => transform.localScale = size;
}
