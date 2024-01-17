using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEffect : EffectParticle
{
    public override void OnActivate()
    {
        base.OnActivate();
        objectType = ObjectPool.ObjectType.Portal;
    }

    public void SetSize(Vector3 size) => transform.localScale = size * 2;
}
