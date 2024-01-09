using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayable : PlayableCtrl
{
    [SerializeField]
    private ObjectPool bulletObjectPool;
        
    public float tempBulletSpeed = 50f; // ���� Ŭ�������� �޾ƿ����� �����Ұ�.

    protected override void OnEntityDied()
    {

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
