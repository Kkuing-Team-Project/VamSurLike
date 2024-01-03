using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyCtrl : Entity
{

    protected NavMeshAgent nav;
    protected PlayableCtrl playable;
    protected Transform enemyPool;

    protected override void InitEntity()
    {
        base.InitEntity();
        if (playable == null)
            playable = FindObjectOfType<PlayableCtrl>();
        if (nav == null)
            nav = gameObject.GetComponent<NavMeshAgent>();
        if (enemyPool == null)
        {
            enemyPool = GameObject.Find("EnemyPool").transform;
            transform.SetParent(enemyPool);
        }
    }

    protected override void UpdateEntity()
    {
        base.UpdateEntity();
        var origin = (Vector2)transform.position;
        var target = (Vector2)playable.transform.position;
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
        gameObject.SetActive(false);
    }
}
