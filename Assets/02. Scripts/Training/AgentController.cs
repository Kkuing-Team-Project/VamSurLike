using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentController : Agent
{
    // public EnvironmentParameters m_ResetParams;
    // public BossController bc;

    // public float playerProximityReward = 0.1f; // 플레이어와 가까워질 때의 보상
    // public float playerDistancePunishment = -0.1f; // 플레이어에서 멀어질 때의 보상

    // public float dangerRadius = 10.0f; // 원의 반지름

    // public bool episodeFinished = false;

    // public override void OnEpisodeBegin()
    // {
    //     bc.ResetBoss();
    //     // 에피소드 시작 시 추가 초기화 작업 수행
    //     episodeFinished = false;
    // }

    // public override void Initialize()
    // {
    //     // boss 구성 요소 가져오기.
    //     bc = GetComponent<BossController>();
    // }

    // public override void CollectObservations(VectorSensor sensor)
    // {
    //     // 현재 보스의 위치를 관측값으로 추가
    //     sensor.AddObservation(boss.transform.position);

    //     // 플레이어와의 거리에 따라 보상을 주기 위해 추가
    //     Collider[] colliders = Physics.OverlapSphere(boss.transform.position, 10f, LayerMask.GetMask("Player"));
    //     if (colliders.Length > 0)
    //     {
    //         float playerDistance = Vector3.Distance(boss.transform.position, colliders[0].transform.position);

    //         // 플레이어와 가까워질 때 양수 보상 부여
    //         if (playerDistance < 5f)
    //         {
    //             this.AddReward(playerProximityReward);
    //         }
    //         // 플레이어에서 멀어질 때 음수 보상 부여
    //         else
    //         {
    //             this.AddReward(playerDistancePunishment);
    //         }
    //     }
    // }

    // public override void OnActionReceived(ActionBuffers actionBuffers)
    // {
    //     // 나는 boss가 y축 빼고 움직였으면 좋겠어
    //     float moveAction = actionBuffers.ContinuousActions[0];
    //     boss.MoveBoss(moveAction);
    // }

    // void OnCollisionEnter(Collision collision)
    // {
    //     // 임의의 원형 좌표 생성
    //     Vector3 centerOfCircle = new Vector3(0, 0, 0);
    //     Vector3 collisionPoint = collision.contacts[0].point;

    //     // 충돌 지점과 원의 중심 간의 거리 계산
    //     float distanceToCenter = Vector3.Distance(centerOfCircle, collisionPoint);

    //     if (distanceToCenter > dangerRadius)
    //     {
    //         // 원형 지름 10의 영역을 넘어갈 경우
    //         float negativeReward = -1f;
    //         this.AddReward(negativeReward);

    //         // 에피소드 종료
    //         this.EndEpisode();
    //     }
    // }
}
