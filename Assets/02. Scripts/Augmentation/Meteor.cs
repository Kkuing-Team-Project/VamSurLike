using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Augmentation
{
    public float splashDamageRadius = 5f; // 스플래쉬 피해 반경
	public GameObject meteorPrefab; // 메테오 프리팹
    private Collider[] enemies; // 모든 적을 저장할 배열

    public Meteor(PlayableCtrl player, int level, AugmentationEventType eventType) : base(player, level, eventType)
	{

	}

	public override void AugmentationEffect(Entity sender, EventArgs e)
	{
        
	}

	private IEnumerator AttackCoroutine(Vector3 targetPosition)
    {

        while (true)
        {
            yield return new WaitForSeconds(6f);


            Collider[] enemies = Physics.OverlapSphere(targetPosition, splashDamageRadius, 1 << LayerMask.NameToLayer("ENEMY"));

            if(enemies.Length > 0)
			{
                int tryCnt = 0;
				while (tryCnt < 100)
				{
                    int randomEnemyIdx = UnityEngine.Random.Range(0, enemies.Length);
                    if (Util.IsTargetInSight(Camera.main.transform, enemies[randomEnemyIdx].transform, Camera.main.fieldOfView))
                    {
                        LaunchMeteorAttack(enemies[randomEnemyIdx].transform.position);
                        break;
                    }
                    else
                        tryCnt++;
				}
			}
        }
    }

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(2f);
    }

    void LaunchMeteorAttack(Vector3 targetPosition)
    {

        // 메테오 프리팹을 생성하여 타겟 위치에 떨어뜨림
        GameObject meteor = UnityEngine.Object.Instantiate(meteorPrefab, targetPosition, Quaternion.identity);

        

        // 스플래쉬 피해 반경 내의 적에게 피해를 줌
        Collider[] colliders = Physics.OverlapSphere(targetPosition, splashDamageRadius); 

        foreach (var enemy in UnityEngine.Object.FindObjectsOfType<EnemyCtrl>())
        {
            enemy.TakeDamage(enemy.hp);
        }

        // 2초 후에 메테오 삭제
        UnityEngine.Object.Destroy(meteor, 2f);
    }
}
