using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHead : EnemyCtrl, IPoolable
{
    public float speed = 1f;  // Speed value
    public float attackPower = 2;  // Attack power value
    public float HP = 10f;
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
        TakeDamage(this, stat.Get(StatType.MAX_HP));
    }

    protected override void OnEntityDied()
    {
        base.OnEntityDied();
        ReturnObject(); // Return the enemy to the pool
    }

    public void ReturnObject()
    {
        pool?.ReturnObject(gameObject, ObjectPool.ObjectType.StoneHead);
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        Debug.Log($" 공격자 : {caster}, StoneHead Hp : {hp}");
    }
}
