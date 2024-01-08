using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentController : Agent
{
    public EnvironmentParameters environmentParameters;
    public BossController bc;

    public float playerProximityReward = 0.1f; // 플레이어와 가까워질 때의 보상
    public float playerDistancePunishment = -0.1f; // 플레이어에서 멀어질 때의 보상

    public float dangerRadius = 10.0f; // 원의 반지름

    public bool episodeFinished = false;

    public override void Initialize()
    {
        // boss 구성 요소 가져오기.
        bc = GetComponent<BossController>();
    }

    public override void OnEpisodeBegin()
    {
        // bc.ResetBoss();
        // // 에피소드 시작 시 추가 초기화 작업 수행
        episodeFinished = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 현재 보스의 위치를 관측값으로 추가
        sensor.AddObservation(bc.transform.position);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // 나는 boss가 y축 빼고 움직였으면 좋겠어
        float moveAction = actionBuffers.ContinuousActions[0];
        // bc.MoveBoss(moveAction);
    }

    public void EndEpisode(float reward)
    {
        AddReward(reward);
        episodeFinished = true; // 에피소드 활성회
        StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        // 학습과정에서 성공 여부를 보려고 1초 딜레이를 줌
        yield return new WaitForSeconds(2f);
        EndEpisode();
    }
}
