// AgentController.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentController : Agent
{
    public BossController boss;
    
    public override void OnEpisodeBegin()
    {
        boss.ResetBoss();
        // 에피소드 시작 시 추가 초기화 작업 수행
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 관측값을 여기에 추가
        // 예를 들어, 보스의 현재 위치 등
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float moveAction = actionBuffers.ContinuousActions[0];
        boss.MoveBoss(moveAction);
    }

    // 필요에 따라 추가적인 메서드나 변수를 정의할 수 있습니다.

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // 벽과 충돌했을 때의 처리
            // 예: 보상 부여, 에피소드 종료 등
        }
    }
}
