using System.Collections;
using UnityEngine;

public class Shield : Augmentation
{
	public int maxShield = 1;
	public int curShield;
	public ShieldEffect shield;
	private ObjectPool pool;
	private Transform playerTransform;
	
	public Shield(int level, int maxLevel) : base(level, maxLevel)
	{
		pool = ObjectPoolManager.Instance.objectPool;
		playerTransform = GameManager.instance.player.transform;
		curShield = 0;
		CoroutineHandler.StartCoroutine(NumberOfShields());
	}

	protected override AugmentationEventType GetEventType()
	{
		return AugmentationEventType.ON_DAMAGE;
	}

	public override void AugmentationEffect(Entity sender, AugEventArgs e) 
	{
		if (curShield > 0)
		{
			e.target.AddEffect(new Invincible(1,Time.deltaTime, e.target));
			curShield--;
			if (curShield <= 0)
			{
				curShield = 0;
				shield?.ReturnObject();
			}
		}
	}

	private IEnumerator NumberOfShields()
	{
		while (true)
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
			
			if (curShield == 0)
			{
				shield = pool.GetObject(ObjectPool.ObjectType.Shield, playerTransform.position).GetComponent<ShieldEffect>();
				shield.transform.SetParent(playerTransform);
			}

			curShield = Mathf.Clamp(curShield + 1, 0, maxShield);
		}
	}

}
