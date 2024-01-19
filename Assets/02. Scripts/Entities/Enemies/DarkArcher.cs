using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkArcher : EnemyCtrl
{
    public Transform arrowposition;
    private Coroutine attackCor;

    protected override void InitEntity()
    {
        base.InitEntity();
    }

    protected override void UpdateEntity()
    {
        base.UpdateEntity();
        float velocity = rigid.velocity.magnitude;

        if(rigid.isKinematic){
            animator.SetFloat("Velocity", velocity);
            rigid.isKinematic = false;
        } 

        // Rigidbody의 속도를 사용하여 애니메이션 속도 매개변수 설정
        animator.SetFloat("Velocity", velocity);
    }

    public override void OnCreate()
    {
        base.OnCreate();
    }

    protected override void EnemyMove()
    {
        if(attackCor == null) base.EnemyMove();
    }

    protected override void EnemyAttack()
    {
        if (attackCor == null) 
            attackCor = StartCoroutine(AttackCor());
    }

    protected override void OnEntityDied()
    {
        base.OnEntityDied();
        ReturnObject(); // Return the enemy to the pool
    }

    public override void ReturnObject()
    {
        pool?.ReturnObject(gameObject, ObjectPool.ObjectType.DarkArcher);
        if(attackCor != null)
        {
            StopCoroutine(attackCor);
            attackCor = null;
        }
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is the Player
        if (collision.transform.TryGetComponent(out Entity entity) && entity == target)
        {
            // Deal damage to the player
            target.TakeDamage(this, stat.Get(StatType.DAMAGE));
            TakeDamage(this, stat.Get(StatType.MAX_HP));

            // Handle DarkArcher's death
            // OnEntityDied();
        }
    }

    private IEnumerator AttackCor()
    {
        animator.SetTrigger("Shoot");
        yield return new WaitUntil(() => IsAnimationClipPlaying("Shoot", 0) == true);
        ShootArrow(transform.forward);

        yield return new WaitUntil(() => IsAnimationClipPlaying("Shoot", 0) == false);

        yield return new WaitForSeconds(0.5f);
        
        while (Vector3.Angle(transform.forward, target.transform.position - transform.position) > 1f)
        {
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3);
            yield return null;
        }
        attackCor = null;
    }

    private void ShootArrow(Vector3 dir)
    {
        GameObject arrowObject = objectPool.GetObject(ObjectPool.ObjectType.Arrow, arrowposition.position);
        TempArrow tempArrow = arrowObject.GetComponent<TempArrow>();
        if (tempArrow != null)
        {
            tempArrow.SetTarget(this, target);
        }

        Rigidbody arrowRigidbody = arrowObject.GetComponent<Rigidbody>();
        arrowRigidbody.velocity = dir * 50f;
        arrowObject.transform.rotation = Quaternion.LookRotation(dir);
        Destroy(arrowObject, 3f);
    }
}

