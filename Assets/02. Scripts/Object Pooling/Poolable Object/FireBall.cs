using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : EffectParticle
{
    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.FireBall;
    }
}
