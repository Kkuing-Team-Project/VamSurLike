using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayable : PlayableCtrl
{
    [SerializeField]
    ObjectPool bulletObjectPool;
        
    public float tempBulletSpeed = 50f; // Temporary bullet speed, can be adjusted in the inspector.

    protected override void OnEntityDied()
    {

    }

    protected override void PlayerAttack()
    {
        TempBullet bullet = bulletObjectPool.Pop(ObjectPool.ObjectType.Bullet).GetComponent<TempBullet>();
        bullet.transform.rotation = Quaternion.LookRotation(transform.forward);

        bullet.rigid.velocity = tempBulletSpeed * transform.forward;
        bullet.transform.position = transform.position;
    }

    protected override void PlayerSkill()
    {
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;

            // Draws a green wireframe sphere to visualize attack distance
            Gizmos.DrawWireSphere(transform.position, stat.Get(StatType.ATTACK_DISTANCE));
        }        
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {

    }
}
