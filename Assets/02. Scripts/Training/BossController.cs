using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Rigidbody rb;
    public BossSpawner bs;
    public AgentController ac;
    public Renderer squareRenderer; // 정사각형 오브젝트의 렌더러
    private Vector3 initialPosition; // 보스 캐릭터의 초기 위치를 저장할 변수

    public float moveSpeed = 5f;

    public bool reset = false; // 리셋 비활성화
    public bool stop = false; // 멈추기 비활성화
    public bool fx = false; // fx = 비활성화

    void Start()
    {
        ac = GetComponent<AgentController>(); // 에이전트 구성요소 받기
        rb = GetComponent<Rigidbody>(); // 물리엔진 구성요소 받기
        bs = GetComponent<BossSpawner>(); // 에이전트 구성요소 받기

        // 시작 시 초기 위치를 저장
        initialPosition = transform.position;
    }

    public void ResetBoss()
    {
        reset = true;
    }

    void UpdateSquareColor(Color color)
    {
        squareRenderer.material.color = color;
    }

    public void MoveBoss(float moveAction)
    {
        // 이동 코드
        Vector3 moveDirection = new Vector3(0f, 0f, moveAction);
        rb.velocity = moveDirection * moveSpeed;
    }

    private void FixedUpdate()
    {
        // 만약 한 에피소드가 끝나면
        if (ac.episodeFinished)
        {
            // 나중에 추가 기능 할거임ㅋ
            UpdateSquareColor(Color.white);
            return;
        }

        if (reset)
        {
            reset = false;
            stop = false;
            return; 
        }
        float distanceToCenter = Vector3.Distance(Vector3.zero, transform.position);

        float distanceToInitialPosition = Vector3.Distance(transform.position, initialPosition);   // 보스가 초기 위치로 되돌아가는 로직 추가

        // 플레이어와 가까워졌을 때 true 반환
        Collider[] colliders2 = Physics.OverlapSphere(transform.position, 5.0f, LayerMask.GetMask("PLAYER"));

        // 플레이어와 멀어졌을 때 true 반환
        Collider[] colliders3 = Physics.OverlapSphere(transform.position, 10.0f, LayerMask.GetMask("PLAYER"));
        
        if (Time.timeSinceLevelLoad > 10f)
        {
            // 40%의 음수 보상 부여
            ac.AddReward(-0.4f);
            UpdateSquareColor(Color.red); // 시간 초과 시 빨간색으로 변경
            ac.EndEpisode(0);
        }

        if (distanceToCenter > 20f)
        {
            UpdateSquareColor(Color.red); // 원 밖으로 나갔을 때 빨간색으로 변경
            print("원 밖으로 나감 ㅋ");
            // 원의 둘레를 벗어날 때 가장 안 좋은 음수 보상을 부여
            ac.AddReward(-1.5f);
            ac.EndEpisode(-1); // 에피소드 종료, 보상은 -1로 설정
        }
       
        if (colliders2.Length > 0)
        {
            ac.AddReward(0.1f);
            UpdateSquareColor(Color.green); // 플레이어와 가까워질 때 초록색으로 변경
        }

        
        if (colliders3.Length == 0)
        {
            ac.AddReward(-0.2f);
            UpdateSquareColor(Color.yellow); // 플레이어와 멀어질 때 노란색으로 변경
        }

        if (stop)
        {
            print("스탑");
            UpdateSquareColor(Color.white);
            return;
        }
    }

    void Update()
    {
        // float moveAction = 1.0f; // 모든 보스가 같은 이동 입력을 받도록 수정
        float moveAction = 0f;
        MoveBoss(moveAction); // 보스 캐릭터 이동
    }

    void OnCollisionEnter(Collision collision)
    {

        // 에피소드가 끝나거나 멈추면 리턴
        if (ac.episodeFinished || stop)
        {
            return;
        }

        if (collision.gameObject.CompareTag("PLAYER"))
        {
            print("맞음");
            UpdateSquareColor(Color.blue);
            ac.AddReward(1f);
            ac.EndEpisode(1000);
        }

        else
        {
            stop = true;   
            UpdateSquareColor(Color.red);
            ac.AddReward(-1f);
            ac.EndEpisode(-1);
        }
    }
}