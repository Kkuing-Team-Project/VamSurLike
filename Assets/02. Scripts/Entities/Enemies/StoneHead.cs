using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHead : EnemyCtrl, IPoolable
{
    public float speed = 1f;  // Speed value
    public float attackPower = 2;  // Attack power value
    public float HP = 10f;
    public Queue<GameObject> pool { get; set; }

    protected override void InitEntity()
    {
        base.InitEntity();
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

    public void Create(Queue<GameObject> pool)
    {
        this.pool = pool;
    }

    public void ReturnObject()
    {
        gameObject.SetActive(false);
        
        pool?.Enqueue(gameObject);
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        Debug.Log($" 공격자 : {caster}, StoneHead Hp : {hp}");
    }
}
