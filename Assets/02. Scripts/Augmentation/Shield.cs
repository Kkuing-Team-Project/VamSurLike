using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Augmentation
{
	public int maxShield = 1;
	public int curShield = 3;
	public GameObject shield;

	public Shield(int level, AugmentationEventType eventType) : base(level, eventType)
	{
		CoroutineHandler.StartCoroutine(NumberOfShields());
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

		}
		shield.gameObject.SetActive(curShield > 0);
	}

	private IEnumerator NumberOfShields()
	{
		yield return new WaitForSeconds(60f);

		switch (level)
		{
			case 1:
				maxShield = 1;
				break;
			case 2:
				maxShield = 2;
				break;
			case 3:
				maxShield = 3;
				break;
			case 4:
				maxShield = 4;
				break;
			case 5:
				maxShield = 5;
				break;
		}

		curShield++;

		if (curShield >= maxShield)
			curShield = maxShield;
	}

}
