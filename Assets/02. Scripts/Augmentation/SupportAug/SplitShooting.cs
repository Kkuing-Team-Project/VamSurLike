using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SplitShooting : Augmentation
{
	public SplitShooting(int level, int maxLevel) : base(level, maxLevel)
	{

	}

	protected override AugmentationEventType GetEventType()
	{
		return AugmentationEventType.ON_ATTACK;
	}

	public override void AugmentationEffect(Entity sender, AugEventArgs e)
	{
		var data = GameManager.instance.augTable[level]["SplitShooting"].ToString().Split("*");
		int num = int.Parse(data[0]);
		int interval = int.Parse(data[1]);

		(e.target as PlayableCtrl).bulletNum = num + 1;
        (e.target as PlayableCtrl).bulletInterval = interval;
	}
}
