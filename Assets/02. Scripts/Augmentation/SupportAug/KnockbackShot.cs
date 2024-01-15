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
        float knockbackForce = 0; // 넉백 힘

        switch (level)
        {
            case 1:
                knockbackForce = 0.2f;
                break;
            case 2:
                knockbackForce = 0.4f;
                break;
            case 3:
                knockbackForce = 0.6f;
                break;
            case 4:
                knockbackForce = 0.8f;
                break;
            case 5:
                knockbackForce = 1f;
                break;
        }
        knockbackForce = 5f;
        if (e.target.TryGetComponent(out Rigidbody rigid))
        {
            Vector3 knockbackDirection = e.eventTr.forward * -1f;
            e.target.AddEffect(new Stun(1, 0.2f, e.target));
            rigid.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }
}
