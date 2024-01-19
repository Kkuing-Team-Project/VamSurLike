using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHead : EnemyCtrl
{
    protected override void InitEntity()
    {
        base.InitEntity();
    }

    protected override void UpdateEntity()
    {
        base.UpdateEntity();
        if(rigid.isKinematic){
            rigid.isKinematic = false;
        }
    }

    protected override void EnemyAttack()
    {
        target.TakeDamage(this, stat.Get(StatType.DAMAGE));
        TakeDamage(this, stat.Get(StatType.MAX_HP));
    }

    public override void ReturnObject()
    {
        base.ReturnObject();
        pool?.ReturnObject(gameObject, ObjectPool.ObjectType.StoneHead);
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
    }
}
