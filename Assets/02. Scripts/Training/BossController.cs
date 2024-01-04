using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // public Rigidbody rb;
    // public AgentController ac;
    // public Renderer squareRenderer; // 정사각형 오브젝트의 렌더러
    // private Vector3 initialPosition; // 보스 캐릭터의 초기 위치를 저장할 변수

    // public float moveSpeed = 5f;

    // public bool reset = false; // 리셋 비활성화
    // public bool stop = false; // 멈추기 비활성화
    // public bool fx = false; // fx = 비활성화

    // // 월드 좌표  0, 0, 0을 기// 랜덤.범위 -9 ~ 9에서  보스가 100개 정도 생성되었으면 좋겠어
    // /*
    //                            (1)
    //                             -  (2)
    //                         -       -  (3)
    //                     -               -  (4)
    //                 _                       -  ...
    //             _                               -
    //                 _                       -
    //                     -               -
    //                         -       -
    //                             - 

    // */
   

    // void Start()
    // {
    //     ac = GetComponent<AgentController>(); // 에이전트 구성요소 받기
    //     rb = GetComponent<Rigidbody>(); // 물리엔진 구성요소 받기

    //     // 시작 시 초기 위치를 저장
    //     initialPosition = transform.position;
    // }

    // // 리셋보스 -> 나중에 에이전트 컨트롤러에서 씀
    // public void ResetBoss()
    // {
    //     transform.position = initialPosition;
    //     reset = true;  
    // }

    // void UpdateSquareColor(Color color)
    // {
    //     // 정사각형 오브젝트의 색깔 변경
    //     squareRenderer.material.color = color;
    // }

    // public void MoveBoss(float moveAction)
    // {
    //     // 이동 코드
    //     Vector3 moveDirection = new Vector3(0f, 0f, moveAction);
    //     rb.velocity = moveDirection * moveSpeed;
    // }

    // private void FixedUpdate()
    // {
    //     // 만약 한 에피소드가 끝나면
    //     if (bc.episodeFinished)
    //     {
    //         // 나중에 추가 기능 할거임ㅋ
    //         UpdateSquareColor(Color.pink); // 시간 초과 시 빨간색으로 변경
    //         return;
    //     }

    //     if (reset)
    //     {
    //         // 랜덤.범위 -9 ~ 9
    //         float positionTheta = Random.Range(-Mathf.PI, Mathf.PI);
    //         float rotationTheta = Random.Range(-Mathf.PI, Mathf.PI);

    //         reset = false; // 리셋 비활성화
    //         stop = false; // stop 비활성화

    //         landingZoneNormal.SetActive(true);  // 기본 목표물 활성화
    //         landingZoneSuccess.SetActive(false); // 성공 목표물 비활성화
    //         landingZoneFail.SetActive(false); // 실패 목표물 비활성화
    //         return; 
    //     }

      
    //     float distanceToInitialPosition = Vector3.Distance(transform.position, initialPosition);   // 보스가 초기 위치로 되돌아가는 로직 추가
    //     bool collidedWithPlayer = CheckCollisionWithPlayer();  // 플레이어와의 충돌 여부 확인
    //     bool timeElapsed = CheckTimeElapsed();  // 10초 안에 플레이어와 충돌하지 않았을 경우 음수 보상 부여
    //     bool outOfCircle = CheckOutOfCircle(); // 원형 밖으로 나갔을 경우 음수 보상 부여
    //     bool playerProximity = CheckPlayerProximity(); // 플레이어와 가까워졌을 때 10%의 보상 부여
    //     bool playerDistanceIncreased = CheckPlayerDistanceIncreased();  // 플레이어와 멀어졌을 때 -20% 음수 보상 부여

    //     if (collidedWithPlayer)
    //     {
    //         // 보상 최대점 부여
    //         ac.AddReward(1f);
    //         UpdateSquareColor(Color.blue); // 플레이어와 충돌 시 파란색으로 변경
    //         return true;
    //     }
        
    //     if (timeElapsed)
    //     {
    //         // 40%의 음수 보상 부여
    //         ac.AddReward(-0.4f);
    //         UpdateSquareColor(Color.red); // 시간 초과 시 빨간색으로 변경
    //         return true;
    //     }

    //     if (outOfCircle)
    //     {
    //         // 음수 보상 최하점 부여
    //         ac.AddReward(-1f);
    //         UpdateSquareColor(Color.red); // 원 밖으로 나갔을 때 빨간색으로 변경
    //         return true;
    //     }

       
    //     if (playerProximity)
    //     {
    //         ac.AddReward(0.1f);
    //         UpdateSquareColor(Color.green); // 플레이어와 가까워질 때 초록색으로 변경
    //     }

        
    //     if (playerDistanceIncreased)
    //     {
    //         ac.AddReward(-0.2f);
    //         UpdateSquareColor(Color.yellow); // 플레이어와 멀어질 때 노란색으로 변경
    //     }
    //     // 여기에 원하는 로직을 추가
    //     float moveAction = Input.GetAxis("Horizontal"); // 가로 방향의 입력을 받음
    //     MoveBoss(moveAction); // 보스 캐릭터 이동
    // }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         // 플레이어와 충돌한 경우 양수 보상 부여
    //         float reward = 1f;
    //         ac.AddReward(reward);

    //         // 충돌 시 에피소드 종료 여부 확인
    //         bool shouldEndEpisode = CheckEpisodeCompletion();
    //         if (shouldEndEpisode)
    //         {
    //             // 종료 시 추가 보상 또는 패널티 부여 (예: 에피소드 종료 보상 또는 음수 보상)
    //             float episodeEndReward = 10f;
    //             ac.AddReward(episodeEndReward);
    //             // 에피소드 종료
    //             ac.EndEpisode();
    //         }
    //     }
    // }

    // bool CheckEpisodeCompletion()
    // {
    //     Debug.Log("CheckTimeElapsed called");
    //     // 보스가 초기 위치로 되돌아가는 로직 추가
    //     float distanceToInitialPosition = Vector3.Distance(transform.position, initialPosition);

    //     // 플레이어와의 충돌 여부 확인
    //     bool collidedWithPlayer = CheckCollisionWithPlayer();

    //     // 10초 안에 플레이어와 충돌하지 않았을 경우 음수 보상 부여
    //     bool timeElapsed = CheckTimeElapsed();

    //     // 원형 밖으로 나갔을 경우 음수 보상 부여
    //     bool outOfCircle = CheckOutOfCircle();

    //     // 종료 조건을 만족하는 경우 에피소드 종료
    //     if (collidedWithPlayer)
    //     {
    //         // 보상 최대점 부여
    //         ac.AddReward(1f);
    //         UpdateSquareColor(Color.blue); // 플레이어와 충돌 시 파란색으로 변경
    //         return true;
    //     }
        
    //     if (timeElapsed)
    //     {
    //         // 40%의 음수 보상 부여
    //         ac.AddReward(-0.4f);
    //         UpdateSquareColor(Color.red); // 시간 초과 시 빨간색으로 변경
    //         return true;
    //     }

    //     if (outOfCircle)
    //     {
    //         // 음수 보상 최하점 부여
    //         ac.AddReward(-1f);
    //         UpdateSquareColor(Color.red); // 원 밖으로 나갔을 때 빨간색으로 변경
    //         return true;
    //     }

    //     // 플레이어와 가까워졌을 때 10%의 보상 부여
    //     bool playerProximity = CheckPlayerProximity();
    //     if (playerProximity)
    //     {
    //         ac.AddReward(0.1f);
    //         UpdateSquareColor(Color.green); // 플레이어와 가까워질 때 초록색으로 변경
    //     }

    //     // 플레이어와 멀어졌을 때 -20% 음수 보상 부여
    //     bool playerDistanceIncreased = CheckPlayerDistanceIncreased();
    //     if (playerDistanceIncreased)
    //     {
    //         ac.AddReward(-0.2f);
    //         UpdateSquareColor(Color.yellow); // 플레이어와 멀어질 때 노란색으로 변경
    //     }

    //     return false;
    // }

    // bool CheckCollisionWithPlayer()
    // {
    //     Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f, LayerMask.GetMask("Player"));
    //     return colliders.Length > 0;
    // }

    // bool CheckTimeElapsed()
    // {
    //     // 10초 안에 충돌하지 않았을 경우 true 반환
    //     bool timeElapsed = Time.timeSinceLevelLoad > 10f;

    //     return timeElapsed;
    // }

    // bool CheckOutOfCircle()
    // {
    //     // 임의의 원 밖으로 나갔을 경우 true 반환
    //     Vector3 centerOfCircle = new Vector3(0, 0, 0);
    //     float circleRadius = 10f;
    //     float distanceToCenter = Vector3.Distance(centerOfCircle, transform.position);
    //     return distanceToCenter > circleRadius;
    // }

    // bool CheckPlayerProximity()
    // {
    //     // 플레이어와 가까워졌을 때 true 반환
    //     Collider[] colliders = Physics.OverlapSphere(transform.position, 5.0f, LayerMask.GetMask("Player"));
    //     return colliders.Length > 0;
    // }

    // bool CheckPlayerDistanceIncreased()
    // {
    //     // 플레이어와 멀어졌을 때 true 반환
    //     Collider[] colliders = Physics.OverlapSphere(transform.position, 10.0f, LayerMask.GetMask("Player"));
    //     return colliders.Length == 0;
    // }
}
