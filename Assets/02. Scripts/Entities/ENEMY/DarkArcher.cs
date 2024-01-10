using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkArcher : EnemyCtrl, IPoolable
{
    public float HP = 20f;
    public float speed = 1.5f;  // Speed value
    public float attackPower = 1f;  // Attack power value
    public float test = 1f;
    public float targetAttackDistance = 10f; // 플레이어 인식 타겟 거리
    public float arrowSpeed = 50f; //총알 스피드
    public Transform arrowposition;

    ObjectPool objectPool; // 화살을 관리할 오브젝트 풀

    public Stack<GameObject> pool { get; set; }
    private Coroutine attackCor;

    protected override void InitEntity()
    {
        base.InitEntity();
        stat.SetDefault(StatType.MOVE_SPEED, speed); // Set the MOVE_SPEED stat
        stat.SetDefault(StatType.DAMAGE, attackPower); // Set the ATTACK_POWER stat
        stat.SetDefault(StatType.ATTACK_DISTANCE, targetAttackDistance);

    }

    protected override void EnemyAttack()
    {
        if(attackCor == null)
            attackCor = StartCoroutine(AttackCor());
    }

    protected override void OnEntityDied()
    {
        // Handle death logic here, e.g., play animations, sound effects, etc.
        Push(); // Return the enemy to the pool
    }

    public void Create(Stack<GameObject> pool)
    {
        this.pool = pool;
        objectPool = GameObject.FindObjectOfType<ObjectPool>().GetComponent<ObjectPool>();
    }

    public void Push()
    {
        gameObject.SetActive(false);
        
        pool?.Push(gameObject);
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {

    }

    protected override void UpdateEntity()
    {
        base.UpdateEntity();
        float velocity = nav.velocity.magnitude; // vector -> 거리
        animator.SetFloat("Velocity", velocity);
    }  

    protected override void EnemyMove(){
        if(attackCor == null)
            base.EnemyMove();
    }

    private IEnumerator AttackCor(){
        
        // animator.SetTrigger("chargeArrowLoop");
        // yield return new WaitUntil(() => IsAnimationClipPlaying("chargeArrowLoop", 0) == false);
        // ShootArrow();

        animator.SetTrigger("Shoot");
        yield return new WaitUntil(() => IsAnimationClipPlaying("Shoot", 0) == true);
        ShootArrow();

        playable.TakeDamage(this, stat.Get(StatType.DAMAGE));
        yield return new WaitUntil(() => IsAnimationClipPlaying("Shoot", 0) == false);

        attackCor = null;
    }
    

    private void ShootArrow()
    {   
        // 오브젝트 풀에서 화살을 가져옴
        // GameObject arrowObject = bulletObjepoolctPool.Pop(ObjectPool.ObjectType.Bullet, spawnPosition, Quaternion.identity);
        GameObject arrowObject = objectPool.Pop(ObjectPool.ObjectType.Arrow, arrowposition.position);

        
        // 화살의 Rigidbody를 가져와 발사 방향과 속도를 설정
        Rigidbody arrowRigidbody = arrowObject.GetComponent<Rigidbody>();
        Vector3 targetDirection = (playable.transform.position - arrowposition.position).normalized; // 플레이어를 향하는 방향
        arrowRigidbody.velocity = targetDirection * arrowSpeed;
        arrowObject.transform.rotation = Quaternion.LookRotation(targetDirection);

        // 화살이 일정 시간 후에 파괴되도록 설정
        Destroy(arrowObject, 3f);
    }
}

