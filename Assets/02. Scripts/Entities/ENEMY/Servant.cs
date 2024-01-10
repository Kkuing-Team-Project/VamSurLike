using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : EnemyCtrl, IPoolable
{
    public float HP = 10f;
    public float speed = 3f;  // Speed value
    public float attackPower = 1f;  // Attack power value
    public float test = 1f;
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
        Push();
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
        Debug.Log($" 공격자 : {caster}, Servant Hp : {hp}");
    }
}