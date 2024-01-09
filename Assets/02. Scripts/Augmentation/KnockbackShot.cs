using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackShot : Augmentation
{
    public float knockbackForce = 0.2f; // 넉백 힘
    public int lvl = 0;

  public KnockbackShot(int level, AugmentationEventType eventType) : base(level, eventType)
	{
        
	}
	public override void AugmentationEffect(Entity sender, AugEventArgs e)
	{
        e.target.GetComponent<Rigidbody>();
	}


    public void Knockback(Entity target, Vector3 direction)
    {
        Rigidbody enemyRigidbody = target.GetComponent<Rigidbody>();

        if (enemyRigidbody != null)
        {
            // 방향으로 힘을 가해 넉백을 발생시킴
            enemyRigidbody.AddForce(direction * knockbackForce, ForceMode.Impulse);
            // 공격을 가할 때 방향 계산 (예를 들어, 플레이어가 바라보는 방향 등)
            Vector3 attackDirection = target.transform.forward;
        }

        switch (lvl)
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
    }

}
