using System.Collections;
using UnityEngine;

public class Meteor : Augmentation
{
    private float time = 1.0f;
    private float meteorDmg = 10f; // 메테오 데미지
    private float splashDamageRadius = 5f; // 스플래쉬 피해 반경
    private float enemiesFindRadius = 20f;
    private float skillTime = 0f;
    private readonly ObjectPool pool;
    private Collider[] enemies; // 모든 적을 저장할 배열
    private Coroutine cor;

    public Meteor(int level, int maxLevel) : base(level, maxLevel)
    {
        pool = ObjectPoolManager.Instance.objectPool;
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_START;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
	{
        skillTime = float.Parse(GameManager.instance.augTable[level]["Meteor"].ToString());
        if (cor != null)
            CoroutineHandler.StopCoroutine(cor);
        cor = CoroutineHandler.StartCoroutine(AttackCoroutine(e.target));
    }

    private IEnumerator AttackCoroutine(Entity player)
    {
        while (true)
        {



            Collider[] enemies = Physics.OverlapSphere(player.transform.position, enemiesFindRadius, 1 << LayerMask.NameToLayer("ENEMY"));
            if(enemies.Length > 0)
			{
                int randomEnemyIdx = UnityEngine.Random.Range(0, enemies.Length);
                CoroutineHandler.StartCoroutine(LaunchMeteorAttack(enemies[randomEnemyIdx].transform.position, player));
			}
            yield return new WaitForSeconds(skillTime);
        }
    }
    
    private IEnumerator LaunchMeteorAttack(Vector3 targetPosition, Entity player)
    {
        pool.GetObject(ObjectPool.ObjectType.Meteor, targetPosition);
        yield return new WaitForSeconds(time);
        Collider[] col = Physics.OverlapSphere(targetPosition, splashDamageRadius, 1 << LayerMask.NameToLayer("ENEMY"));

        if(col.Length > 0)
		{
		    foreach (var enemy in col)
		    {
                enemy.GetComponent<Entity>().TakeDamage(player, meteorDmg);
		    }
		}


    }
}
