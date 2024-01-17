using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : StatusEffect
{
    Material freezeMaterial;
    public Freeze(Material freezeMaterial, int level, float duration, Entity caster = null) : base(level, duration, caster)
    {
        this.freezeMaterial = freezeMaterial;
    }

    public override void OnStart(Entity target)
    {
        target.rigid.velocity = Vector3.zero;
        target.rigid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        Material[] materials = new Material[target.originMaterials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = freezeMaterial;
        }
        target.meshRenderer.materials = materials;

        target.SetAnimationPlaying(false);
    }

    public override void OnUpdate(Entity target)
    {
    }

    public override void OnFinish(Entity target)
    {
        target.rigid.constraints = RigidbodyConstraints.FreezeRotation;
        target.ResetMaterial();
        target.SetAnimationPlaying(true);
    }
}
