using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFieldEffect : EffectParticle
{
    [Header("Gas (1) Particle"), SerializeField]
    ParticleSystem firstGasParticle;
    [Header("Gas (2) Particle"), SerializeField]
    ParticleSystem secondGasParticle;

    void SetEffectSize(float size)
    {
        ParticleSystem.EmissionModule emission = firstGasParticle.emission;
        emission.rateOverTime = mainParticle.emission.rateOverTime.constant * size;

        ParticleSystem.ColorOverLifetimeModule temp = mainParticle.colorOverLifetime;
        temp.enabled = false;
    }
}
