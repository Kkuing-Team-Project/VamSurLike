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
        target.rigid.isKinematic = true;
        target.rigid.velocity = Vector3.zero;

        SkinnedMeshRenderer skinnedMeshRenderer = target.GetComponentInChildren<SkinnedMeshRenderer>();
        //Material material = skinnedMeshRenderer.material;

        //material = freezeMaterial;

        skinnedMeshRenderer.material = freezeMaterial;
    }

    public override void OnUpdate(Entity target)
    {
    }

    public override void OnFinish(Entity target)
    {
        target.rigid.isKinematic = false;

        SkinnedMeshRenderer skinnedMeshRenderer = target.GetComponentInChildren<SkinnedMeshRenderer>();

        skinnedMeshRenderer.material = target.OriginMaterial;
    }
}
