using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using UnityEngine;

public abstract class PlayableCtrl : Entity
{
    public int level { get; private set; }
    public float exp { get; private set; }
    public float requireExp { get; private set; } = 10;

    public event AugmentationDelegate OnUpdateAugmentation;
    public event AugmentationDelegate OnAttackPlayer;

    [Tooltip("초당 회전 각도 값")]
    public float rotationAnglePerSecond;

    [Tooltip("대쉬 거리"), Range(1, 10)]
    public float dashDist = 60;
    [Range(0.01f, 1)]
    public float dashTime;
    private Vector3 inputVector;
    private Coroutine attackCor;
    private Coroutine dashCor;
    private List<Augmentation> augmentationList = new List<Augmentation>();

    protected override void InitEntity()
    {
        base.InitEntity();
        stat.SetDefault(StatType.MOVE_SPEED, 3);
    }

    void FixedUpdate()
    {
        rigid.velocity = inputVector.normalized * stat.Get(StatType.MOVE_SPEED);
    }

    protected override void UpdateEntity()
    {
        base.UpdateEntity();
        OnUpdateAugmentation?.Invoke(this, EventArgs.Empty);

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.z = Input.GetAxis("Vertical");

        // 공격 범위 내에 적이 있다면.
        if(GetNearestEnemy() != null)
        {
            #region Look Nearst Enemy

            Vector3 targetDirection = (GetNearestEnemy().transform.position - transform.position).normalized;   // 적을 향한 벡터
            Quaternion nextRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), rotationAnglePerSecond * Time.deltaTime);  // 다음 프레임에 적용할 회전값

            transform.rotation = nextRotation;

            #endregion

            if (attackCor == null)
            {
                attackCor = StartCoroutine(AttackCoroutine());
            }
        }


        // 공격 범위 내에 적이 없다면
        else
        {
            if(attackCor != null)
            {
                StopCoroutine(attackCor);
                attackCor = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerSkill();
        }

        if (Input.GetKeyDown(KeyCode.Space) && dashCor == null)
        {
            dashCor = StartCoroutine(DashCor());
        }
    }
    
    /// <summary>
    /// 가장 근접한 적을 반환하는 메서드
    /// </summary>
    /// <returns></returns>
    protected EnemyCtrl GetNearestEnemy()
    {
        var enemies = Physics.OverlapSphere(transform.position, stat.Get(StatType.ATTACK_DISTANCE), 1 << LayerMask.NameToLayer("ENEMY"));
        if (enemies.Length > 0)
        {
            EnemyCtrl result = enemies[0].GetComponent<EnemyCtrl>();
            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(transform.position, result.transform.position) > Vector3.Distance(transform.position, enemy.transform.position))
                {
                    result = enemy.GetComponent<EnemyCtrl>();
                }
            }
            return result;
        }
        else return null;
    }

    protected IEnumerator DashCor()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), transform.position.y, Input.GetAxisRaw("Vertical")).normalized;

        float radius = gameObject.GetComponent<CapsuleCollider>().radius;

        float eT = 0;
        Vector3 origin = transform.position;
        Vector3 moveTo = Vector3.zero;
        if (Physics.SphereCast(transform.position, radius, dir, out RaycastHit hit, dashDist, 1 << LayerMask.NameToLayer("ENEMY")))
        {
            moveTo = hit.point + (-dir * radius);
            while(eT < dashTime)
            {
                yield return null;
                eT += Time.deltaTime;
                transform.position = Vector3.Lerp(origin, moveTo, eT / dashTime);
            }
        }
        else
        {
            moveTo = dir * dashDist;
            while (eT < dashTime)
            {
                yield return null;
                eT += Time.deltaTime;
                transform.position = Vector3.Lerp(origin, moveTo, eT / dashTime);
            }
        }
        dashCor = null;
    }

    protected abstract void PlayerSkill();


    protected abstract void PlayerAttack();

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

    //증강 추가 메소드
    public void AddAugmentation(Augmentation aug)
    {
        switch (aug.eventType)
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

    //증강 삭제(클래스에 따라)
    public void DeleteAugmentation(Augmentation aug)
    {
        if (aug.eventType == AugmentationEventType.ON_START || augmentationList.Count <= 0)
            return;

        Augmentation del = augmentationList.Find((a) => a.GetType() == aug.GetType());

        if (del == null)
            return;

        switch (aug.eventType)
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

    //증강 삭제(이름과 호출 타입 필요)
    public void DeleteAugmentation(string augName, AugmentationEventType type)
    {
        if (type == AugmentationEventType.ON_START || augmentationList.Count <= 0)
            return;

        Augmentation del = augmentationList.Find((a) => string.Equals(a.GetType().Name, augName));

        if (del == null)
            return;

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

    public void SetExperienceValue(float val)
    {
        exp += val;
        if(exp >= requireExp)
        {
            exp = exp - requireExp;
            level++;
        }
    }
}
