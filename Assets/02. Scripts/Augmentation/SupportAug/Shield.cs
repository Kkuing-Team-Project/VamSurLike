using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : Augmentation
{
	public int maxShield;
    public ShieldEffect shield;
	public int curShield;
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
		if (GameManager.instance.player.HasAugmentation<Shield>() == false)
		{
            pool = ObjectPoolManager.Instance.objectPool;
            playerTransform = GameManager.instance.player.transform;
            maxShield = int.Parse(GameManager.instance.augTable[level]["Shield"].ToString());
            curShield = maxShield;
            shield = pool.GetObject(ObjectPool.ObjectType.Shield, playerTransform.position).GetComponent<ShieldEffect>();
			shield.transform.SetParent(playerTransform);
            shield.SetColor(colors[curShield - 1]);
        }
		else
		{
			var nowShield = GameManager.instance.player.GetAugmentation<Shield>() as Shield;
            nowShield.maxShield = int.Parse(GameManager.instance.augTable[nowShield.level + 1]["Shield"].ToString());
            nowShield.curShield = int.Parse(GameManager.instance.augTable[nowShield.level + 1]["Shield"].ToString());
            nowShield.shield.gameObject.SetActive(true);
            nowShield.shield.SetColor(colors[nowShield.curShield - 1]);
            Debug.Log(maxShield);
		}
    }

	protected override AugmentationEventType GetEventType()
	{
		return AugmentationEventType.ON_DAMAGE;
	}

	public override void AugmentationEffect(Entity sender, AugEventArgs e) 
	{
        if (curShield > 0)
		{
			e.target.AddEffect(new Invincible(1, Time.deltaTime, e.target));
			CoroutineHandler.StartCoroutine(RegenerateShield(60));
            curShield--;
            shield.SetColor(colors[Mathf.Clamp(curShield - 1, 0, maxShield)]);
		}

        if (curShield <= 0)
        {
            shield.gameObject.SetActive(false);
        }
    }

	private IEnumerator RegenerateShield(float time)
	{
		yield return new WaitForSeconds(time);
        curShield = Mathf.Clamp(curShield + 1, 0, maxShield);
        if (curShield > 0)
        {
            shield.gameObject.SetActive(true);
        }
        shield.SetColor(colors[curShield - 1]);
    }
}
