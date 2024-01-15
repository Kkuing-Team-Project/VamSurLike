using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : EffectParticle
{
    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.HitParticle;
    }
}
