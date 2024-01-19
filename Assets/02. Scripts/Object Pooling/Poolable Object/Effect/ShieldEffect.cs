using static UnityEngine.Rendering.DebugUI;
using UnityEngine;

public class ShieldEffect : EffectParticle
{
    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.Shield;
    }

    public void SetColor(Color32 color)
    {
        ParticleSystem[] particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particleSystems)
        {
            var main = particle.main;
            particle.startColor = color;
        }
    }
}
