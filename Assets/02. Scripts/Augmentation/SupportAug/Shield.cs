using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : Augmentation
{
	private int maxShield;
	private int curShield;
	private ShieldEffect shield;
	private ObjectPool pool;
	private Transform playerTransform;
	private Color32[] colors = {
		new (255, 0, 0, 10), // red
		new (255, 255, 0, 10), // Orange
		new (255, 165, 0, 10), // yellow
		new (0, 255, 0, 10), // green
		new (0, 0, 255, 10) // blue
	};
	
	public Shield(int level, int maxLevel) : base(level, maxLevel)
	{
		pool = ObjectPoolManager.Instance.objectPool;
		playerTransform = GameManager.instance.player.transform;
		CoroutineHandler.StartCoroutine(NumberOfShields());
        maxShield = int.Parse(GameManager.instance.augTable[level]["Shield"].ToString());
		curShield = maxShield;
		shield = pool.GetObject(ObjectPool.ObjectType.Shield, playerTransform.position).GetComponent<ShieldEffect>();
		shield.transform.SetParent(playerTransform);
        SetColor(colors[curShield]);
    }

	protected override AugmentationEventType GetEventType()
	{
		return AugmentationEventType.ON_DAMAGE;
	}

	public override void AugmentationEffect(Entity sender, AugEventArgs e) 
	{
        maxShield = int.Parse(GameManager.instance.augTable[level]["Shield"].ToString());
        if (curShield > 0)
		{
			if(shield == null)
			{
				shield = pool.GetObject(ObjectPool.ObjectType.Shield, playerTransform.position).GetComponent<ShieldEffect>();
            }
			e.target.AddEffect(new Invincible(1,Time.deltaTime, e.target));
			curShield--;

		}

        if (curShield <= 0)
        {
            curShield = 0;
            shield.ReturnObject();
        }
		else
		{
            SetColor(colors[curShield]);
        }
    }

	private IEnumerator NumberOfShields()
	{
		while (true)
		{
			yield return new WaitForSeconds(3f);
			
			curShield = Mathf.Clamp(curShield + 1, 0, maxShield);
			SetColor(colors[curShield - 1]);
		}
	}

	private void SetColor(Color32 color)
	{
		ParticleSystem[] particleSystems = shield.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particle in particleSystems)
		{
			particle.startColor = color;
		}
	}

}
