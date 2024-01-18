using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBoltEffect : EffectParticle
{
    private CinemachineImpulseSource source;
    public override void OnCreate()
    {
        base.OnCreate();
        source = gameObject.GetComponent<CinemachineImpulseSource>();
        objectType = ObjectPool.ObjectType.LightningBolt;
    }

    public override void OnActivate()
    {
        base.OnActivate();
        source.GenerateImpulse();
    }
}
