using System;
using UnityEngine;

public class MeteorEffect : EffectParticle
{
    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.Meteor;
    }

    public void OnEnable()
    {
        
    }
}
