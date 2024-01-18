using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSlowEffect : EffectParticle
{
    [Header("Gas (1) Particle"), SerializeField]
    ParticleSystem firstGasParticle;
    [Header("Gas (2) Particle"), SerializeField]
    ParticleSystem secondGasParticle;

    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.PoisonField;
    }
    public override void OnActivate()
    {
        base.OnActivate();
    }
}
