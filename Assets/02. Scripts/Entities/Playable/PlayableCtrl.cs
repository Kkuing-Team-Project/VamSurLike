using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class PlayableCtrl : Entity
{
    public event AugmentationDelegate OnUpdateAugmentation;
    public event AugmentationDelegate OnAttackPlayer;

    [Range(1f, 3f)]
    public float rotSpeed;

    private Vector3 dir;
    private Coroutine attackCor;
    private List<Augmentation> augmentationList = new List<Augmentation>();

    protected override void InitEntity()
    {
        base.InitEntity();
        stat.SetDefault(StatType.MOVE_SPEED, 3);
    }

    protected override void UpdateEntity()
    {
        base.UpdateEntity();
        OnUpdateAugmentation?.Invoke(this, EventArgs.Empty);

        //Player Move
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        transform.Translate(dir * stat.Get(StatType.MOVE_SPEED) * Time.deltaTime, Space.World);
        //

        //Player Attack
        if(GetNearestEnemy() != null)
        {
            //È¸Àü
            Vector3 targetDir = (GetNearestEnemy().transform.position - transform.position).normalized;
            Vector3 lookAtDir = Vector3.RotateTowards(transform.forward, targetDir, rotSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(lookAtDir);
            if (attackCor == null)
                attackCor = StartCoroutine(AttackCoroutine());
        }
        else
        {
            if(attackCor != null)
            {
                StopCoroutine(attackCor);
                attackCor = null;
            }
        }
        //
    }

    protected EnemyCtrl GetNearestEnemy()
    {
        var enemies = Physics.OverlapSphere(transform.position, stat.Get(StatType.ATTACK_DISTANCE), 1 << LayerMask.NameToLayer("ENEMY"));
        if (enemies.Length > 0)
        {
            EnemyCtrl result = enemies[0].GetComponent<EnemyCtrl>();
            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(transform.position, result.transform.position) > Vector3.Distance(transform.position, enemy.transform.position))
                    result = enemy.GetComponent<EnemyCtrl>();
            }
            return result;
        }
        else return null;
    }

    private IEnumerator AttackCoroutine()
    {
        WaitForSeconds attackDelay = new WaitForSeconds(1 / stat.Get(StatType.ATTACK_SPEED));
        while (true)
        {
            OnAttackPlayer?.Invoke(this, EventArgs.Empty);
            PlayerAttack();
            yield return attackDelay;
        }
    }

    protected abstract void PlayerAttack();

    //
    public void AddAugmentation(Augmentation aug, AugmentationEventType type)
    {
        switch (type)
        {
            case AugmentationEventType.ON_START:
                aug.AugmentationEffect(this, EventArgs.Empty);
                break;
            case AugmentationEventType.ON_UPDATE:
                OnUpdateAugmentation += aug.AugmentationEffect;
                break;
            case AugmentationEventType.ON_ATTACK:
                OnAttackPlayer += aug.AugmentationEffect;
                break;
            default:
                break;
        }
        augmentationList.Add(aug);
    }

    public void DeleteEvent(Augmentation aug, AugmentationEventType type)
    {
        if (type == AugmentationEventType.ON_START || augmentationList.Count <= 0)
            return;

        Augmentation del = augmentationList.Find((a) => a.GetType() == aug.GetType());

        switch (type)
        {
            case AugmentationEventType.ON_UPDATE:
                OnUpdateAugmentation -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            case AugmentationEventType.ON_ATTACK:
                OnUpdateAugmentation -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            default:
                break;
        }
        augmentationList.Remove(del);
    }
}
