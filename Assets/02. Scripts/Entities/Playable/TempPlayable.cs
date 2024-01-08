using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayable : PlayableCtrl
{
    [SerializeField]
    ObjectPool bulletObjectPool;
        
    public float tempBulletSpeed = 50f; // 무기 클래스에서 받아오도록 변경할것.

    protected override void OnEntityDied()
    {

    }

    protected override void PlayerAttack()
    {
        TempBullet bullet = bulletObjectPool.Pop().GetComponent<TempBullet>();
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

            Gizmos.DrawWireSphere(transform.position, stat.Get(StatType.ATTACK_DISTANCE));
        }        
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {

    }
}
