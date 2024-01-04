using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public Transform target; // 플레이어나 목표 지점의 Transform
    public float disappearDistance = 1f; // 플레이어와의 거리가 이 값 이하일 때 사라짐

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (target == null)
        {
            Debug.LogError("Target not assigned to EnemyAI script!");
        }
        else
        {
            SetDestination();
        }
    }

 
// AI script
    void Update()
    {
        // Add debug logs for testing
        // Debug.Log("Distance to target: " + Vector3.Distance(transform.position, target.position));

        // 만약 목표 지점이 이동 중에 변경되었다면 다시 설정
        if (target.hasChanged)
        {
            SetDestination();
            target.hasChanged = false;
        }

        // 플레이어와의 거리를 체크하여 일정 거리 이하로 다가가면 사라짐
        if (Vector3.Distance(transform.position, target.position) <= disappearDistance)
        {
            Destroy(gameObject);
        }
    }

    void SetDestination()
    {
        if (target != null)
        {
            navMeshAgent.SetDestination(target.position);
        }
    }
}
