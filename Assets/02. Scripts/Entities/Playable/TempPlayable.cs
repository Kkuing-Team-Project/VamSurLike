using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
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
        TempBullet bullet = bulletObjectPool.Pop(ObjectPool.ObjectType.Bullet, transform.position).GetComponent<TempBullet>();
        if (bullet == null)
        {
            return;
        }
        bullet.transform.rotation = Quaternion.LookRotation(transform.forward);

        bullet.rigid.velocity = tempBulletSpeed * transform.forward;
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
}
