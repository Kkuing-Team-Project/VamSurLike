using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : StatusEffect
{
    Material freezeMaterial;
    public Freeze(Material freezeMaterial, int level, float duration, Entity caster = null) : base(level, duration, caster)
    {
        this.freezeMaterial = freezeMaterial;
        //this.level = level;
        //this.duration = duration;

    }

    public override void OnStart(Entity target)
    {
        target.rigid.velocity = Vector3.zero;
        target.rigid.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        SkinnedMeshRenderer skinnedMeshRenderer = target.GetComponentInChildren<SkinnedMeshRenderer>();

        skinnedMeshRenderer.material = freezeMaterial;
    }

    public override void OnUpdate(Entity target)
    {
    }

    public override void OnFinish(Entity target)
    {
        target.rigid.constraints = RigidbodyConstraints.FreezeRotation;
        target.ResetMaterial();
    }
}
