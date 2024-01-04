using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public void MoveBoss(float moveAction)
    {
        // 이동 코드를 여기에 작성
        // moveAction은 -1에서 1까지의 값으로, 이를 이용하여 이동 방향을 결정
        float moveAmount = moveAction * moveSpeed * Time.fixedDeltaTime;
        transform.Translate(Vector3.forward * moveAmount);
    }

    public void ResetBoss()
    {
        // 보스의 추가 초기화 작업을 수행
        // 예를 들어, 체력 초기화 등
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어와 충돌한 경우 양수 보상 부여
            float reward = 1f;
            // 보상을 주는 부분 추가
            // AgentController에게 보상을 전달하는 코드를 작성
            // 예: agentController.AddReward(reward);
        }
        else
        {
            // 다른 물체와 충돌한 경우 음수 보상 부여
            float reward = -1f;
            // 보상을 주는 부분 추가
            // AgentController에게 보상을 전달하는 코드를 작성
            // 예: agentController.AddReward(reward);
        }
    }
}
