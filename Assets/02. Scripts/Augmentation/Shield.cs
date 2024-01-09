using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Augmentation
{
	public int lvl = 0;
	public int maxShield = 1;
	public int curShield = 0;

	public Shield(PlayableCtrl player, int level, AugmentationEventType eventType) : base(player, level, eventType)
	{
		
	}

	public override void AugmentationEffect(Entity sender, EventArgs e)
	{
		
	}

	private IEnumerator NumberOfShields()
	{
		yield return new WaitForSeconds(60f);

		switch (lvl)
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
