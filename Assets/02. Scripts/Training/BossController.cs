// BossController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class BossController : Entity
{
    public Rigidbody rb;
    public AgentController ac;
    public Renderer squareRenderer; // 정사각형 오브젝트의 렌더러
    private Vector3 initialPosition; // 보스 캐릭터의 초기 위치를 저장할 변수

    public new float hp = 1000f; // 체력
    public float moveSpeed = 1.5f; // 이동 속도
    public float attackPower = 2f; // 공격력
    public float meleeAttackDistance = 2.0f; // 근거리 공격 거리
    public float longAttackDistance = 6.0f; // 장거리 공격 거리

    public bool reset = false; // 리셋 비활성화
    public bool stop = false; // 멈추기 비활성화
    public bool fx = false; // fx = 비활성화

    #region
    public float waitTime; // 대기 시간 변수

    #endregion

    protected int nowPatternIdx; // 현재 패턴 인덱스

    protected delegate IEnumerator PatternDelegate(); // 패턴을 위한 대리자(delegate) 선언

    protected List<PatternDelegate> patternList = new List<PatternDelegate>(); // 패턴 리스트

    protected NavMeshAgent nav; // 네비게이션 에이전트
    protected PlayableCtrl playable;  // 플레이어블 컨트롤러
    protected Transform enemyPool; // 적들을 관리하는 풀

    protected Coroutine attackPatternCor; // 공격 패턴

    void Start()
    {
        ac = GetComponent<AgentController>(); // 에이전트 구성요소 받기
        rb = GetComponent<Rigidbody>(); // 물리엔진 구성요소 받기
        //bs = GetComponent<BossSpawner>(); // 에이전트 구성요소 받기

        // 시작 시 초기 위치를 저장
        initialPosition = transform.position;
    }

    // 엔티티 초기화 메서드: 보스 객체가 생성되거나 게임이 시작될 때 호출됩니다.
    protected override void InitEntity()
    {
        // 상위 클래스(Entity)의 InitEntity 메서드를 호출하여 기본적인 초기화 수행
        base.InitEntity();

        // playable 변수가 설정되지 않았다면, PlayableCtrl 컴포넌트를 찾아서 할당합니다.
        // 이는 일반적으로 플레이어 캐릭터를 제어하는 컴포넌트를 찾는데 사용됩니다.
        if (playable == null)
            playable = FindObjectOfType<PlayableCtrl>();

        // nav 변수가 설정되지 않았다면, 이 게임 오브젝트에 부착된 NavMeshAgent 컴포넌트를 찾아서 할당합니다.
        // NavMeshAgent는 보스의 경로 계획 및 이동에 사용됩니다.
        if (nav == null)
            nav = gameObject.GetComponent<NavMeshAgent>();

        // enemyPool 변수가 설정되지 않았다면, "EnemyPool"이라는 이름의 오브젝트를 찾아서 그의 transform을 할당합니다.
        // 이는 일반적으로 적들을 관리하는 풀(pool)을 찾는데 사용됩니다. 이렇게 하면 모든 적 개체를 효율적으로 관리할 수 있습니다.
        if (enemyPool == null)
        {
            enemyPool = GameObject.Find("EnemyPool").transform;
            // 보스 객체를 enemyPool의 자식으로 설정합니다. 이는 씬 계층을 깔끔하게 유지하고,
            // 적들을 일괄적으로 관리하기 쉽게 만들어줍니다.
            transform.SetParent(enemyPool);
        }
        // nowPatternIdx 변수에 랜덤한 패턴 인덱스를 할당합니다. 
        // 이는 보스의 초기 공격 패턴을 랜덤하게 선택하기 위해 사용됩니다.
        nowPatternIdx = Random.Range(0, patternList.Count);
    }

    protected void RegisterPatterns(params PatternDelegate[] pattern) // 패턴을 등록하는 메서드
    {
        foreach (PatternDelegate patternDelegate in pattern)
        {
            patternList.Add(patternDelegate);
        }
    }

    protected override void UpdateEntity() // 업데이트 메서드
    {
        base.UpdateEntity(); // Entity 클래스의 업데이트 메서드 호출
        // 플레이어와의 거리를 계산하여 보스의 이동 또는 공격 패턴을 결정합니다.
        var origin = transform.position; 
        origin.y = 0;
        var target = playable.transform.position;
        target.y = 0;
        float distanceToPlayer = Vector3.Distance(origin, target);

        // 공격 거리 밖에 있으면 보스 이동
        if (distanceToPlayer > longAttackDistance && attackPatternCor == null)
            BossMove();
        else if (distanceToPlayer <= meleeAttackDistance) // 근거리 공격 조건
        {
            if (attackPatternCor == null)
                attackPatternCor = StartCoroutine(BossPatternCor());
        }
        else if (distanceToPlayer <= longAttackDistance) // 장거리 공격 조건
        {
            if (attackPatternCor == null)
                attackPatternCor = StartCoroutine(BossPatternCor());
        }

        BossAnimation();
    }

    // protected override void UpdateEntity()
    // {
    //     base.UpdateEntity();
    //     var origin = transform.position;
    //     origin.y = 0;
    //     var target = playable.transform.position;
    //     target.y = 0;
    //     if (Vector3.Distance(origin, target) > stat.Get(StatType.ATTACK_DISTANCE) && attackPatternCor == null)
    //         BossMove();
    //     else
    //     {
    //         if (attackPatternCor == null)
    //             attackPatternCor = StartCoroutine(BossPatternCor());
    //     }
    //     BossAnimation();
    // }

    protected virtual void BossAnimation()
    {
        if(animator != null)
        {
            animator.SetFloat("Velocity", nav.velocity.normalized.magnitude);
        }
    }

    protected virtual void BossMove() // 보스 이동 메서드
    {
        nav.speed = stat.Get(StatType.MOVE_SPEED);
        nav.stoppingDistance = stat.Get(StatType.ATTACK_DISTANCE);
        nav.SetDestination(playable.transform.position);
    }
 
    protected IEnumerator BossPatternCor()  // 보스 패턴 코루틴
    {
        // 애니메이션 레이어 변경 (패턴에 맞게)
        yield return ChangeAnimLayer(nowPatternIdx + 1, 0.1f, true);
        yield return StartCoroutine(patternList[nowPatternIdx].Invoke());
        // 애니메이션 레이어 복원 (Idle 상태로)
        yield return ChangeAnimLayer(nowPatternIdx + 1, waitTime, false);
        // 다음 패턴 선택
        nowPatternIdx = Random.Range(0, patternList.Count);
        attackPatternCor = null;
    }

    // 애니메이션 레이어를 변경하는 코루틴
    protected IEnumerator ChangeAnimLayer(int layer, float time, bool increasing)
    {
        if (animator == null)
            yield break;

        float dT = 0;
        while(dT < time)
        {
            dT += Time.deltaTime;
            yield return null;
            float val = increasing ? dT / time : 1f - dT / time;

            animator.SetLayerWeight(layer, val);
        }

    }

    protected override void OnEntityDied()
    {
        // 보스 사망 시 실행할 로직을 여기에 작성합니다.
        // 예: 사망 애니메이션 재생, 로그 메시지 출력, 게임 오브젝트 비활성화 등
        Debug.Log("Boss is dead!");

        // 추가적인 사망 처리 로직을 구현하세요.
        // 예를 들어, 사망 애니메이션 재생이나 게임 오브젝트 비활성화 등

        gameObject.SetActive(false); // 보스 오브젝트를 비활성화합니다.
    }

    // Entity 클래스에서 상속받은 OnTakeDamage 메서드를 구현합니다.
    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        // 보스가 피해를 받을 때 실행할 로직을 여기에 작성합니다.
        // 예: 피해량 만큼 체력 감소, 피해 반응 애니메이션 재생 등
        Debug.Log($"Boss took {dmg} damage from {caster.gameObject.name}!");

        // 체력을 감소시킵니다.
        hp -= dmg;

        // 체력이 0 이하면 사망 처리
        if (hp <= 0)
        {
            OnEntityDied();
        }

        // 피해 반응 애니메이션 등 추가적인 로직을 구현하세요.
        // 예: animator.SetTrigger("Hurt") 등
    }

}
