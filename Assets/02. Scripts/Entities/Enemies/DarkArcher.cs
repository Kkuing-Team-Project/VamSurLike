using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkArcher : EnemyCtrl, IPoolable
{
    public float HP = 20f;
    public float speed = 1.5f;  // Speed value
    public float attackPower = 1f;  // Attack power value
    public float targetAttackDistance = 10f; // 플레이어 인식 타겟 거리
    public float arrowSpeed = 50f; //총알 스피드
    public Transform arrowposition;

    public ObjectPool pool { get; set; }

    private Coroutine attackCor;

    protected override void InitEntity()
    {
        base.InitEntity();
    }
    public void OnCreate()
    {
    }

    public void OnActivate()
    {
    }

    protected override void EnemyAttack()
    {
        if(attackCor == null) attackCor = StartCoroutine(AttackCor());
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is the Player
        if (collision.gameObject.CompareTag("PLAYER"))
        {
            // Deal damage to the player
            var player = collision.gameObject.GetComponent<PlayableCtrl>(); // Replace 'Player' with your player class
            if (player != null)
            {
                playable.TakeDamage(this, stat.Get(StatType.DAMAGE));
                TakeDamage(this, stat.Get(StatType.MAX_HP));
            }

            // Handle DarkArcher's death
            OnEntityDied();
        }
    }

    protected override void OnEntityDied()
    {
        base.OnEntityDied();
        ReturnObject(); // Return the enemy to the pool
    }

    public void ReturnObject()
    {
        pool?.ReturnObject(gameObject, (ObjectPool.ObjectType.DarkArcher));
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
        GameObject arrowObject = objectPool.GetObject(ObjectPool.ObjectType.Arrow, arrowposition.position);
        TempArrow tempArrow = arrowObject.GetComponent<TempArrow>();
        if (tempArrow != null)
        {
            tempArrow.SetArcher(this);
        }

        Rigidbody arrowRigidbody = arrowObject.GetComponent<Rigidbody>();
        arrowRigidbody.velocity = dir * arrowSpeed;
        arrowObject.transform.rotation = Quaternion.LookRotation(dir);
        Destroy(arrowObject, 3f);
    }
}

