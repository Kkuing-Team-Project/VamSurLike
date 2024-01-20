using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBombEffect : EffectParticle
{
    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.PoisonField;
        SoundManager.Instance.PlaySound("Sound_EF_UP_Nuke");
    }
}
