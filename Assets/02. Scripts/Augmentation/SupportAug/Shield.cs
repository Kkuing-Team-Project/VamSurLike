using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Augmentation
{
	public int maxShield = 1;
	public int curShield = 1;
	public Transform shieldTransform;
	private ObjectPool pool;
	private PlayableCtrl player;
	
	public Shield(int level, int maxLevel) : base(level, maxLevel)
	{
		pool = ObjectPoolManager.Instance.objectPool;
		player = GameManager.instance.player;
		CoroutineHandler.StartCoroutine(NumberOfShields());
	}

	protected override AugmentationEventType GetEventType()
	{
		return AugmentationEventType.ON_DAMAGE;
	}

	public override void AugmentationEffect(Entity sender, AugEventArgs e) 
	{
		if (curShield > 0)
		{
			// 무적 추가
			e.target.AddEffect(new Invincible(1,Time.deltaTime, e.target));
			curShield--;
		}
		else
		{
			
		}
	}

	private IEnumerator NumberOfShields()
	{
		yield return new WaitForSeconds(60f);
		shieldTransform =  pool.GetObject(ObjectPool.ObjectType.Shield, Vector3.zero).transform;
		shieldTransform.SetParent(player.transform);
		shieldTransform.position = Vector3.zero;
		
		curShield = Mathf.Clamp(curShield + 1, 0, maxShield);
	}

}
