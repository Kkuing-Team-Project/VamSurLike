using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyCtrl : Entity
{
    protected NavMeshAgent nav;
    protected PlayableCtrl playable;
    protected ObjectPool objectPool;
    protected AugEventArgs enemyArgs;

    protected override void InitEntity()
    {
        base.InitEntity();
        if (playable == null)
            playable = FindObjectOfType<PlayableCtrl>();
        if (nav == null)
            nav = gameObject.GetComponent<NavMeshAgent>();
        if (enemyArgs == null)
            enemyArgs = new AugEventArgs(transform, this);
        playable.InvokeEvent(AugmentationEventType.ON_SPAWN_ENEMY, this, enemyArgs);
        objectPool = FindObjectOfType<ObjectPool>();
    }

    protected override void UpdateEntity()
    {
        playable.InvokeEvent(AugmentationEventType.ON_UPDATE_ENEMY, this, enemyArgs);
        var origin = transform.position;
        origin.y = 0;
        var target = playable.transform.position;
        target.y = 0;
        if (Vector3.Distance(origin, target) > stat.Get(StatType.ATTACK_DISTANCE))
            EnemyMove();
        else
            EnemyAttack();
    }


    protected virtual void EnemyMove()
    {
        nav.speed = stat.Get(StatType.MOVE_SPEED);
        nav.stoppingDistance = stat.Get(StatType.ATTACK_DISTANCE);
        nav.SetDestination(playable.transform.position);
    }

    protected abstract void EnemyAttack();

    protected override void OnEntityDied()
    {
        objectPool.GetObject(ObjectPool.ObjectType.Experience, transform.position);
        GameManager.instance.killCount++;
    }
}
