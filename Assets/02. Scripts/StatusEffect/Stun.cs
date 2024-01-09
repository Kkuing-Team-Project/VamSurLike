using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stun : StatusEffect
{
    public Stun(int level, float duration, Entity caster = null) : base(level, duration, caster)
    {
    }

    public override void OnFinish(Entity target)
    {
        target.rigid.isKinematic = true;
        target.rigid.velocity = Vector3.zero;

        if(target.TryGetComponent(out NavMeshAgent nav))
        {
            nav.velocity = Vector3.zero;
        }
    }

    public override void OnStart(Entity target)
    {
        target.rigid.isKinematic = false;
    }

    public override void OnUpdate(Entity target)
    {
    }
}
