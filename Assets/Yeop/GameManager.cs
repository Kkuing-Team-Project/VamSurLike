using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject aiPrefab; // AI 프리팹
    public Transform player; // 플레이어의 위치를 기반으로 AI를 생성할 위치
    public float spawnRate = 5f; // 1초에 생성되는 AI 개수
    private float nextSpawnTime = 0f; // 다음 AI 생성 시간

    void Update()
    {
        // 현재 시간이 다음 AI 생성 시간보다 크면 AI를 생성
        if (Time.time >= nextSpawnTime)
        {
            SpawnAI();
            nextSpawnTime = Time.time + 1f / spawnRate; // 다음 AI 생성 시간 설정
        }
    }

   void SpawnAI()
    {
        // 플레이어 주변에 AI를 생성하기 위한 랜덤한 위치 생성
        Vector3 spawnPosition = player.position + Random.insideUnitSphere * 5f;

        // AI 생성
        GameObject aiInstance = Instantiate(aiPrefab, spawnPosition, Quaternion.identity);

        // Set the player as the target for the spawned AI
        AI aiScript = aiInstance.GetComponent<AI>();
        if (aiScript != null)
        {
            aiScript.target = player;
        }
        else
        {
            Debug.LogError("AI script not found on the instantiated object!");
        }
    }


    
}
