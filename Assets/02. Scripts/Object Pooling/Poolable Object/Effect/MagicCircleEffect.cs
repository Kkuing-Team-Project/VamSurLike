using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleEffect : EffectParticle
{
    public override void OnActivate()
    {
        base.OnActivate();
        objectType = ObjectPool.ObjectType.MagicCircle;
    }
    public void SetColor(Color color)
    {
        var main = mainParticle.main;
        main.startColor = color;
        var light = transform.GetChild(0).GetComponent<ParticleSystem>().main;
        light.startColor = color;
    }

    public void SetSize(Vector3 size)
    {
        var main = mainParticle.main;
        main.startSize = size.magnitude;
        var lightShape = transform.GetChild(0).GetComponent<ParticleSystem>().shape;
        lightShape.scale = size;
    }
}
