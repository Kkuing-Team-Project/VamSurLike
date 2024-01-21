using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyFieldEffect : EffectParticle
{
    PlayableCtrl player;

    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.EnergyField;
        player = GameManager.instance.player;
    }

    public void SetParticleSize(float size)
    {
        ParticleSystem.ShapeModule shape = mainParticle.shape;
        shape.radius = size;

        ParticleSystem.EmissionModule emission = mainParticle.emission;
        emission.SetBurst(0, new ParticleSystem.Burst(0.000f, 4 * size));
        emission.SetBurst(1, new ParticleSystem.Burst(0.025f, 4 * size));
        emission.SetBurst(2, new ParticleSystem.Burst(0.050f, 4 * size));
    }

    private void FixedUpdate()
    {
        transform.position = player.transform.position + Vector3.up;
    }
}
