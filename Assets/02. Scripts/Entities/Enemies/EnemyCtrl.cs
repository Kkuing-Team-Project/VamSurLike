using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCtrl : Entity, IPoolable
{
    protected PlayableCtrl playable;
    protected Spirit spirit;
    public Entity target { get; protected set; }
    protected ObjectPool objectPool;
    protected AugEventArgs enemyArgs;

    public ObjectPool pool { get; set; }

    protected override void InitEntity()
    {
        base.InitEntity();
        if (playable == null)
            playable = FindObjectOfType<PlayableCtrl>();
        if (spirit == null)
            spirit = FindObjectOfType<Spirit>();
        if (enemyArgs == null)
            enemyArgs = new AugEventArgs(transform, this);
        if (objectPool == null)
            objectPool = FindObjectOfType<ObjectPool>();
        playable.InvokeEvent(AugmentationEventType.ON_SPAWN_ENEMY, this, enemyArgs);
        hp = stat.Get(StatType.MAX_HP);
    }

    protected override void UpdateEntity()
    {
        playable.InvokeEvent(AugmentationEventType.ON_UPDATE_ENEMY, this, enemyArgs);
        var origin = transform.position;
        origin.y = 0;
        target = spirit.spiritState == SpiritState.OCCUPY ? spirit : playable;
        var targetPos = target.transform.position;
        targetPos.y = 0;
        var attackDistance = stat.Get(StatType.ATTACK_DISTANCE);

        if (Vector3.Distance(origin, targetPos) > attackDistance)
            EnemyMove();
        else
            EnemyAttack();
    }
    
    protected virtual void EnemyMove()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float moveSpeed = stat.Get(StatType.MOVE_SPEED);
        rigid.velocity = direction * moveSpeed;

        // 적이 플레이어를 조준
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
    }

    protected abstract void EnemyAttack();

    protected override void OnEntityDied()
    {
        objectPool.GetObject(ObjectPool.ObjectType.Experience, transform.position + Vector3.up);
        GameManager.instance.killCount++;
    }

    public virtual void OnCreate()
    {
        originMaterials = GetComponentInChildren<SkinnedMeshRenderer>()?.materials;
    }

    public virtual void OnActivate()
    {
        InitEntity();
        rigid.velocity = Vector3.zero;
        rigid.isKinematic = false;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        SetAnimationPlaying(true);
        ResetMaterial();      
    }

    public virtual void ReturnObject()
    {  
    }
}
