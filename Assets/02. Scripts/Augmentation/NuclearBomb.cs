using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBomb : Augmentation
{
	public float skillTime = 240f;
	public int lvl = 1;
	private int currentLevel = 0;
	private IEnumerator enumerator;

	public NuclearBomb(PlayableCtrl player, int level, AugmentationEventType eventType) : base(player, level, eventType)
	{
		enumerator = Bomb(true);
		if (lvl < currentLevel)
		{
			CoroutineHandler.StopCoroutine(enumerator);
			skillTime = 0f;
			enumerator = Bomb(false);
			CoroutineHandler.StartCoroutine(enumerator);
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
			enumerator = Bomb(true);
			CoroutineHandler.StartCoroutine(enumerator);
		}
	}

	public override void AugmentationEffect(Entity sender, EventArgs e)
	{
			
	}

	public IEnumerator Bomb(bool isInfinity)
	{
		while (isInfinity)
		{
			yield return new WaitForSeconds(skillTime);

			foreach (var enemy in UnityEngine.Object.FindObjectsOfType<EnemyCtrl>())
			{
				//enemy.TakeDamage(enemy.hp);
				enemy.GetComponent<Entity>().TakeDamage(player, enemy.hp);
			}
		}
	}

}
