using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prion : EnemyCtrl, IPoolable
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
        gameObject.SetActive(false);
        
        pool?.ReturnObject(gameObject, ObjectPool.ObjectType.Prion);
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        Debug.Log($" 공격자 : {caster}, Prion Hp : {hp}");
    }
}