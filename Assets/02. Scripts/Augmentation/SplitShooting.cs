using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitShooting : Augmentation
{
	public int lvl;

	public SplitShooting(int level, AugmentationEventType eventType) : base(level, eventType)
	{

	}

	public override void AugmentationEffect(Entity sender, AugEventArgs e)
	{
		
	}

	public void Update()
	{

	}	

	private void Attack()
	{
		switch (lvl)
		{
			case 1:
				//GameObject bulletL = UnityEngine.Object.Instantiate()

				break;
			case 2:

				break;
			case 3:

				break;
			case 4:

				break;
			case 5:

				break;
		}
	}

}
