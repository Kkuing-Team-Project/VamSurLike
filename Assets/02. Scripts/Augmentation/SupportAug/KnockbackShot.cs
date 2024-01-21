using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackShot : Augmentation
{

    public KnockbackShot(int level, int maxLevel) : base(level, maxLevel)
	{
        
	}

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_HIT;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
	{
        float knockbackForce = float.Parse(GameManager.instance.augTable[level]["KnockbackShot"].ToString());


        if (e.target.TryGetComponent(out Rigidbody rigid))
        {
            Vector3 knockbackDirection = (e.eventTr.position - GameManager.instance.player.transform.position).normalized;
            e.target.AddEffect(new Stun(1, 0.2f, e.target));
            rigid.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }
}
