using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayable : PlayableCtrl
{
    [SerializeField]
    ObjectPool bulletObjectPool;

    protected override void OnEntityDied()
    {

    }

    protected override void PlayerAttack()
    {
        TempBullet bullet = bulletObjectPool.Pop().GetComponent<TempBullet>();
        bullet.rigid.velocity = 10f * transform.forward;
        bullet.transform.position = transform.position;
    }

    protected override void PlayerSkill()
    {
        throw new System.NotImplementedException();
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(transform.position, stat.Get(StatType.ATTACK_DISTANCE));
        }        
    }
}
