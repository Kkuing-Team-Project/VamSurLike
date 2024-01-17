using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyFieldEffect : EffectParticle
{
    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.EnergyField;
    }
}
