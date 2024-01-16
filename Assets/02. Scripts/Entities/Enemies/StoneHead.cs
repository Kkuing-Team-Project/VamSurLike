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

    public override void OnActivate()
    {
        base.OnActivate();
    }

    public override void OnCreate()
    {
        base.OnCreate();
    }

    protected override void EnemyAttack()
    {
        playable.TakeDamage(this, stat.Get(StatType.DAMAGE));
        TakeDamage(this, stat.Get(StatType.MAX_HP));
    }

    protected override void OnEntityDied()
    {
        base.OnEntityDied();
        ReturnObject(); // Return the enemy to the pool
    }

    public override void ReturnObject()
    {
        pool?.ReturnObject(gameObject, ObjectPool.ObjectType.StoneHead);
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
    }
}
