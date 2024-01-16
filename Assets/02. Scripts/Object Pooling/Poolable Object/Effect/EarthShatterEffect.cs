using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthShatterEffect : EffectParticle
{
    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.EarthShatter;
    }

    public void SetStartSpeed(float speed)
    {
        var main = mainParticle.main;
        main.startSpeed = speed;
    }
}
