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

    public Stack<GameObject> pool { get; set; }
    private Coroutine attackCor;

    protected override void InitEntity()
    {
        base.InitEntity();
        stat.SetDefault(StatType.MOVE_SPEED, speed); // Set the MOVE_SPEED stat
        stat.SetDefault(StatType.DAMAGE, attackPower); // Set the ATTACK_POWER stat
        stat.SetDefault(StatType.ATTACK_DISTANCE, targetAttackDistance);
        hp = HP;
    }

    protected override void EnemyAttack()
    {
        if(attackCor == null) attackCor = StartCoroutine(AttackCor());
    }

    protected override void OnEntityDied()
    {
        base.OnEntityDied();
        Push(); // Return the enemy to the pool
    }

    public void Create(Stack<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Push()
    {
        gameObject.SetActive(false);
        pool?.Push(gameObject);
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        Debug.Log($" 공격자 : {caster}, DarkArcher Hp : {hp}");
    }

    protected override void UpdateEntity()
    {
        base.UpdateEntity();
        float velocity = nav.velocity.magnitude; // vector -> 거리
        animator.SetFloat("Velocity", velocity);
    }  

    protected override void EnemyMove(){
        if(attackCor == null) base.EnemyMove();
    }

    private IEnumerator AttackCor()
    {
        var targetDir = transform.forward;
        float rotTime = 0;

        animator.SetTrigger("Shoot");
        yield return new WaitUntil(() => IsAnimationClipPlaying("Shoot", 0) == true);
        ShootArrow(targetDir);

        playable.TakeDamage(this, stat.Get(StatType.DAMAGE));
        yield return new WaitUntil(() => IsAnimationClipPlaying("Shoot", 0) == false);
        
        while(rotTime < 0.9f){
            Vector3 target = playable.transform.position;
            target.y = transform.position.y;
            Vector3 lookAt = (target - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookAt), 180f * Time.deltaTime);
            yield return null;
            rotTime += Time.deltaTime;
        }
        attackCor = null;
    }

    private void ShootArrow(Vector3 dir)
    {   
        GameObject arrowObject = objectPool.Pop(ObjectPool.ObjectType.Arrow, arrowposition.position);
        Rigidbody arrowRigidbody = arrowObject.GetComponent<Rigidbody>();
        arrowRigidbody.velocity = dir * arrowSpeed;
        arrowObject.transform.rotation = Quaternion.LookRotation(dir);
        Destroy(arrowObject, 3f);
    }
}

