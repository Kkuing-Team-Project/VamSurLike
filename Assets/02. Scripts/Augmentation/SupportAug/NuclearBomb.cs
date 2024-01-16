using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBomb : Augmentation
{
	public float skillTime = 240f;

	public NuclearBomb(int level, int maxLevel) : base(level, maxLevel)
	{

		
	}

	protected override AugmentationEventType GetEventType()
	{
		return AugmentationEventType.ON_START;
	}

	public override void AugmentationEffect(Entity sender, AugEventArgs e)
	{
		skillTime = float.Parse(GameManager.instance.augTable[level]["NuclearBomb"].ToString());
        CoroutineHandler.StartCoroutine(Bomb(e.target));
    }

	public IEnumerator Bomb(Entity player)
	{
		while (true)
		{


			foreach (var enemy in UnityEngine.Object.FindObjectsOfType<EnemyCtrl>())
			{
				enemy.GetComponent<Entity>().TakeDamage(player, enemy.hp);
			}

            yield return new WaitForSeconds(skillTime);
		}
	}

}
