using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : EnemyCtrl, IPoolable
{
    public ObjectPool pool { get; set; }

    protected override void InitEntity()
    {
        base.InitEntity();
    }
    public void OnCreate()
    {
    }

    public void OnActivate()
    {
    }

    protected override void EnemyAttack()
    {
        playable.TakeDamage(this, stat.Get(StatType.DAMAGE));
        ReturnObject();
    }

    protected override void OnEntityDied()
    {
        base.OnEntityDied();
        ReturnObject(); // Return the enemy to the pool
    }

    public void ReturnObject()
    {
        pool.ReturnObject(gameObject, ObjectPool.ObjectType.Servant);
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        Debug.Log($" 공격자 : {caster}, Servant Hp : {hp}");
    }
}