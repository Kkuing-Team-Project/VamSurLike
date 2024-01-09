using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prion : EnemyCtrl, IPoolable
{
    public float HP = 20f;
    public float speed = 1f;  // Speed value
    public float attackPower = 1f;  // Attack power value
    public float test = 1f;
    public Stack<GameObject> pool { get; set; }

    protected override void InitEntity()
    {
        base.InitEntity();
        stat.SetDefault(StatType.MOVE_SPEED, speed); // Set the MOVE_SPEED stat
        stat.SetDefault(StatType.DAMAGE, attackPower); // Set the ATTACK_POWER stat
        stat.SetDefault(StatType.ATTACK_DISTANCE, 2f);
    }

    protected override void EnemyAttack()
    {
        Debug.Log("EnemyAttack");
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {

        HP -= dmg; // Reduce HP by the damage amount
        print(HP);
        if (HP <= 0)
        {
            OnEntityDied(); // Call the Die method if HP is 0 or less
        }
    }

    protected override void OnEntityDied()
    {
        // Handle death logic here, e.g., play animations, sound effects, etc.
        Push(); // Return the enemy to the pool
    }

    public void Create(Stack<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Push()
    {
        gameObject.SetActive(false);
        pool.Push(gameObject);
    }
}
