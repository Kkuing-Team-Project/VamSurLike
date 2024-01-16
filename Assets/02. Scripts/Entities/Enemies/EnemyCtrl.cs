using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCtrl : Entity, IPoolable
{
    protected PlayableCtrl playable;
    protected ObjectPool objectPool;
    protected AugEventArgs enemyArgs;

    public ObjectPool pool { get; set; }

    protected override void InitEntity()
    {
        base.InitEntity();
        if (playable == null)
            playable = FindObjectOfType<PlayableCtrl>();
        if (enemyArgs == null)
            enemyArgs = new AugEventArgs(transform, this);
        playable.InvokeEvent(AugmentationEventType.ON_SPAWN_ENEMY, this, enemyArgs);
        objectPool = FindObjectOfType<ObjectPool>();
        hp = stat.Get(StatType.MAX_HP);
    }

    protected override void UpdateEntity()
    {
        playable.InvokeEvent(AugmentationEventType.ON_UPDATE_ENEMY, this, enemyArgs);
        var origin = transform.position;
        var target = playable.transform.position;
        var attackDistance = stat.Get(StatType.ATTACK_DISTANCE);

        if (Vector3.Distance(origin, target) > attackDistance)
            EnemyMove();
        else
            EnemyAttack();
    }
    
    protected virtual void EnemyMove()
    {
        Vector3 direction = (playable.transform.position - transform.position).normalized;
        float moveSpeed = stat.Get(StatType.MOVE_SPEED);
        rigid.velocity = direction * moveSpeed;

        // 적이 플레이어를 조준
        transform.LookAt(new Vector3(playable.transform.position.x, transform.position.y, playable.transform.position.z));
    }

    protected abstract void EnemyAttack();

    protected override void OnEntityDied()
    {
        objectPool.GetObject(ObjectPool.ObjectType.Experience, transform.position);
        GameManager.instance.killCount++;
    }

    public virtual void OnCreate()
    {
    }

    public virtual void OnActivate()
    {
    }

    public virtual void ReturnObject()
    {  
    }
}
