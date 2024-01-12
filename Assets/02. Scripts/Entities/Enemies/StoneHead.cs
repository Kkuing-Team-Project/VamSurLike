using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHead : EnemyCtrl, IPoolable
{
    public float speed = 1f;  // Speed value
    public float attackPower = 2;  // Attack power value
    public float HP = 10f;
    public Stack<GameObject> pool { get; set; }

    protected override void InitEntity()
    {
        base.InitEntity();
        stat.SetDefault(StatType.MOVE_SPEED, speed); // Set the MOVE_SPEED stat
        stat.SetDefault(StatType.DAMAGE, attackPower); // Set the ATTACK_POWER stat
        stat.SetDefault(StatType.ATTACK_DISTANCE, 2f);
        hp = HP;
    }

    protected override void EnemyAttack()
    {
        playable.TakeDamage(this, stat.Get(StatType.DAMAGE));
        TakeDamage(this, stat.Get(StatType.MAX_HP));
    }

    protected override void OnEntityDied()
    {
        base.OnEntityDied();
        Push(); // Return the enemy to the pool
    }

    public void Create(Stack<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Push()
    {
        gameObject.SetActive(false);
        
        pool?.Push(gameObject);
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        Debug.Log($" 공격자 : {caster}, StoneHead Hp : {hp}");
    }
}
