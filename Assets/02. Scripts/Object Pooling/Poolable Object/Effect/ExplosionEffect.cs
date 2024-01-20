using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : EffectParticle
{
    public override void OnActivate()
    {
        base.OnActivate();
        objectType = ObjectPool.ObjectType.Explosion;
    }

    public void SetSize(Vector3 size) => transform.localScale = size;
}
