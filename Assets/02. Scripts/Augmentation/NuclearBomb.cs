using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBomb : Augmentation
{
	public float skillTime = 240f;
	public int lvl = 0;

	public NuclearBomb(PlayableCtrl player, int level, AugmentationEventType eventType) : base(player, level, eventType)
	{
		CoroutineHandler.StartCoroutine(Bomb());
		
	}

	public override void AugmentationEffect(Entity sender, EventArgs e)
	{
			
	}

	public IEnumerator Bomb()
	{
		while (true)
		{
			yield return new WaitForSeconds(skillTime);

			foreach (var enemy in UnityEngine.Object.FindObjectsOfType<EnemyCtrl>())
			{
				enemy.GetComponent<Entity>().TakeDamage(player, enemy.hp);
			}

			switch (lvl)
			{
				case 1:
					skillTime = 240f;
					break;
				case 2:
					skillTime = 210f;
					break;
				case 3:
					skillTime = 180f;
					break;
				case 4:
					skillTime = 150f;
					break;
				case 5:
					skillTime = 120f;
					break;
			}
		}
	}

}
