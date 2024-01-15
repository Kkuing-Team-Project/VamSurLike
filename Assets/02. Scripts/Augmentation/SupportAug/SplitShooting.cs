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
		switch (level)
		{
			case 1:
				(e.target as PlayableCtrl).bulletInterval = 10;
				(e.target as PlayableCtrl).bulletInterval = 2;
				break;
			case 2:
				(e.target as PlayableCtrl).bulletInterval = 20;
				(e.target as PlayableCtrl).bulletInterval = 3;
				break;
			case 3:
				(e.target as PlayableCtrl).bulletInterval = 20;
				(e.target as PlayableCtrl).bulletInterval = 4;
				break;
			case 4:
				(e.target as PlayableCtrl).bulletInterval = 30;
				(e.target as PlayableCtrl).bulletInterval = 5;
				break;
		}
	}
}
