using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Augmentation
{
    public float meteorDmg = 10f; // 메테오 데미지
    public float splashDamageRadius = 5f; // 스플래쉬 피해 반경
    public float enemiesFindRadius = 20f;
    public float skillTime = 0f;
	public GameObject meteorPrefab; // 메테오 프리팹
    private Collider[] enemies; // 모든 적을 저장할 배열

    public Meteor(int level, AugmentationEventType eventType) : base(level, eventType)
	{
        
	}

	public override void AugmentationEffect(Entity sender, AugEventArgs e)
	{
        CoroutineHandler.StartCoroutine(AttackCoroutine(e.target));
    }

    private IEnumerator AttackCoroutine(Entity player)
    {

        while (true)
        {
            yield return new WaitForSeconds(skillTime);


            Collider[] enemies = Physics.OverlapSphere(player.transform.position, enemiesFindRadius, 1 << LayerMask.NameToLayer("ENEMY"));

            if(enemies.Length > 0)
			{
                int tryCnt = 0;
				while (tryCnt < 100)
				{
                    int randomEnemyIdx = UnityEngine.Random.Range(0, enemies.Length);
                    if (Util.IsTargetInSight(Camera.main.transform, enemies[randomEnemyIdx].transform, Camera.main.fieldOfView))
                    {
                        LaunchMeteorAttack(enemies[randomEnemyIdx].transform.position, 3f, player);
                        break;
                    }
                    else
                        tryCnt++;
				}
			}
        }
    }

    private IEnumerator LaunchMeteorAttack(Vector3 targetPosition, float time, Entity player)
    {

        // 메테오 프리팹을 생성하여 타겟 위치에 떨어뜨림
        GameObject meteor = UnityEngine.Object.Instantiate(meteorPrefab, targetPosition + Vector3.up * 50, Quaternion.identity);

        float dT = 0;
        Vector3 origin = targetPosition + Vector3.up * 50;
        while(dT < time)
		{
            meteor.transform.position = Vector3.Lerp(origin, targetPosition, dT / time);
            yield return null;
            dT += Time.deltaTime;
		}

        Collider[] col = Physics.OverlapSphere(targetPosition, splashDamageRadius, 1 << LayerMask.NameToLayer("ENEMY"));


        if(col.Length > 0)
		{
		    foreach (var enemy in col)
		    {
                enemy.GetComponent<Entity>().TakeDamage(player, meteorDmg);
		    }
		}

        switch (level)
        {
            case 1:
                skillTime = 12f;
                break;
            case 2:
                skillTime = 10f;
                break;
            case 3:
                skillTime = 8f;
                break;
            case 4:
                skillTime = 6f;
                break;
            case 5:
                skillTime = 2f;
                break;
        }

    }
}
