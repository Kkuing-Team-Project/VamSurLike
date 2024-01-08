using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBomb : Augmentation
{
	public NuclearBomb(PlayableCtrl player, int level, AugmentationEventType eventType) : base(player, level, eventType)
	{

	}

	public override void AugmentationEffect(Entity sender, EventArgs e)
	{
			
	}

	public IEnumerator Bomb()
	{

		yield return new WaitForSeconds(240f);

		foreach (var enemy in UnityEngine.Object.FindObjectsOfType<EnemyCtrl>())
		{
			enemy.TakeDamage(enemy.hp);
		}
	}
}
