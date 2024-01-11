using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitShooting : Augmentation
{
	public SplitShooting(int level, int maxLevel, AugmentationEventType eventType) : base(level, maxLevel, eventType)
	{

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
