using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject BossPrefab; // 몬스터 프리팹을 할당하기 위한 변수
    public int numberOfBoss = 20; // 소환할 몬스터의 수
    public float circleRadius = 9f; // 원의 반지름

    void Start()
    {
        SpawnBoss();
    }

    void SpawnBoss()
    {
        for (int i = 0; i < numberOfBoss; i++)
        {
            float angle = i * (360f / numberOfBoss);

            Vector3 spawnPosition = new Vector3(
                transform.position.x + circleRadius * Mathf.Sin(Mathf.Deg2Rad * angle),
                transform.position.y,
                transform.position.z + circleRadius * Mathf.Cos(Mathf.Deg2Rad * angle)
            );

            Instantiate(BossPrefab, spawnPosition, Quaternion.identity);
        }
    }
}