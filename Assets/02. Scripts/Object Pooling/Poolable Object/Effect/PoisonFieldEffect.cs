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

    public override void OnActivate()
    {
        base.OnActivate();
    }

    public void SetEffectSize(float size)
    {
        ParticleSystem.ShapeModule shape = mainParticle.shape;
        shape.scale = Vector3.one * size;
        shape = firstGasParticle.shape;
        shape.scale = Vector3.one * size;
        shape = secondGasParticle.shape;
        shape.scale = Vector3.one * size;


        ParticleSystem.EmissionModule emission = mainParticle.emission;
        emission.rateOverTime = mainParticle.emission.rateOverTime.constant * size;
        emission = firstGasParticle.emission;
        emission.rateOverTime = firstGasParticle.emission.rateOverTime.constant * size;
        emission = secondGasParticle.emission;
        emission.rateOverTime = secondGasParticle.emission.rateOverTime.constant * size;
    }
}
