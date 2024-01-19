using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : EnemyCtrl
{
    protected override void InitEntity()
    {
        base.InitEntity();
    }

    public override void OnActivate()
    {
        base.OnActivate();        
    }

    protected override void UpdateEntity()
    {
        base.UpdateEntity();
        if(rigid.isKinematic){
            rigid.isKinematic = false;
        }
    }

    public override void OnCreate()
    {
        base.OnCreate();
    }

    protected override void EnemyAttack()
    {
        target.TakeDamage(this, stat.Get(StatType.DAMAGE));
        TakeDamage(this, stat.Get(StatType.MAX_HP));
    }

    protected override void OnEntityDied()
    {
        base.OnEntityDied();
        ReturnObject(); // Return the enemy to the pool
    }

    public override void ReturnObject()
    {
        base.ReturnObject();
        pool?.ReturnObject(gameObject, ObjectPool.ObjectType.Servant);
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
    }
}