using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : EnemyCtrl, IPoolable
{
    protected override void InitEntity()
    {
        base.InitEntity();
        stat.SetDefault(StatType.ATTACK_DISTANCE, 2f);
    }

    protected override void EnemyAttack()
    {
        Debug.Log("EnemyAttack");
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {

    }
}
