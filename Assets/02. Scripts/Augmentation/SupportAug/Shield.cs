using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Augmentation
{
	public int maxShield = 1;
	public int curShield = 1;
	public GameObject shield;

	public Shield(int level, int maxLevel) : base(level, maxLevel)
	{
		CoroutineHandler.StartCoroutine(NumberOfShields());
	}

	protected override AugmentationEventType GetEventType()
	{
		return AugmentationEventType.ON_DAMAGE;
	}

	public override void AugmentationEffect(Entity sender, AugEventArgs e) 
	{
		//if(shield == null)
		//{
		//	e.eventTr.Find("PlayerCanvas").Find("Shield");
		//}

		if (curShield > 0) // 
		{
			// 무적 추가
			e.target.AddEffect(new Invincible(1,Time.deltaTime, e.target));
			curShield--;
		}
		//shield.gameObject.SetActive(curShield > 0);
	}

	private IEnumerator NumberOfShields()
	{
		yield return new WaitForSeconds(60f);
		curShield = Mathf.Clamp(curShield + 1, 0, maxShield);
	}

}
