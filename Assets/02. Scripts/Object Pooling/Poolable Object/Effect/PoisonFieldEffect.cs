using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFieldEffect : EffectParticle
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

    void SetEffectSize(float size)
    {
        ParticleSystem.EmissionModule emission = firstGasParticle.emission;
        emission.rateOverTime = mainParticle.emission.rateOverTime.constant * size;
    }
}
