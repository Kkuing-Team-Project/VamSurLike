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

    [Header("점멸 속도"), SerializeField]
    float dashSpeed = 40f;

    [Header("점멸 이동 시간"), SerializeField]
    float dashTime;

    // 이동 입력값
    private Vector3 inputVector;

    // 코루틴
    private Coroutine attackCor;
    private Coroutine dashCor;

    // 증강 리스트
    private List<Augmentation> augmentationList = new List<Augmentation>();

    protected override void InitEntity()
    {
        base.InitEntity();
        stat.SetDefault(StatType.MOVE_SPEED, 3);
    }

    void FixedUpdate()
    {
        if (dashCor == null)
        {
            rigid.velocity = inputVector.normalized * stat.Get(StatType.MOVE_SPEED);
        }
    }

    [ContextMenu("증강 추가")]
    public void AddAugmentationTest()
    {
        if (HasAugmentation<DamageUp>())
        {
            Debug.Log("!");
            GetAugmentation<DamageUp>().SetAugmentationLevel(GetAugmentationLevel<DamageUp>() + 1);
        }
        else
        {
            AddAugmentation(new DamageUp(this, 1, AugmentationEventType.ON_UPDATE));
        }
    }



    protected override void UpdateEntity()
    {
        base.UpdateEntity();
        OnUpdateAugmentation?.Invoke(this, EventArgs.Empty);

        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.z = Input.GetAxisRaw("Vertical");

        // 공격 범위 내에 적이 있다면.
        if(GetNearestEnemy() != null)
        {
            #region Look Nearst Enemy
            Vector3 targetPosition = GetNearestEnemy().transform.position;
            targetPosition.y = transform.position.y;
            Vector3 targetDirection = (targetPosition - transform.position).normalized;   // 적을 향한 벡터
            Quaternion nextRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), rotationAnglePerSecond * Time.deltaTime);  // 다음 프레임에 적용할 회전값

            transform.rotation = nextRotation;

            #endregion

            if (attackCor == null)
            {
                attackCor = StartCoroutine(AttackCoroutine());
            }
        }


        // 공격 범위 내에 적이 없다면
        else if (GetNearestEnemy() == null)
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
    protected Entity GetNearestEnemy()
    {
        var radius = stat.Get(StatType.ATTACK_DISTANCE);
        var enemies = Physics.OverlapSphere(transform.position, radius, 1 << LayerMask.NameToLayer("ENEMY"));
        if (enemies.Length > 0)
        {
            Entity result = enemies[0].GetComponent<Entity>();
            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(transform.position, result.transform.position) > Vector3.Distance(transform.position, enemy.transform.position))
                {
                    result = enemy.GetComponent<Entity>();
                }
            }
            return result;
        }
        else
        {
            return null;
        }
    }

    protected IEnumerator DashCor()
    {
        rigid.velocity = Vector3.zero;
        Vector3 direction = inputVector.normalized;

        if (direction == Vector3.zero)
        {
            Vector3 mousePosition = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                mousePosition = hit.point;
            }
            mousePosition.y = transform.position.y;

            direction = mousePosition - transform.position;
            direction.Normalize();
        }

        rigid.velocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashTime);

        rigid.velocity = Vector3.zero;

        dashCor = null;
    }


    protected abstract void PlayerSkill();


    protected abstract void PlayerAttack();

    private IEnumerator AttackCoroutine()
    {
        WaitForSeconds attackDelay = new WaitForSeconds(1 / /*stat.Get(StatType.ATTACK_SPEED)*/5f);
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
    public void DeleteAugmentation<T>() where T : Augmentation
    {
        Augmentation del = augmentationList.Find((a) => a is T);
        
        if (del.eventType == AugmentationEventType.ON_START || augmentationList.Count <= 0)
            return;


        if (del == null)
            return;

        switch (del.eventType)
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

    public Augmentation GetAugmentation<T>() where T : Augmentation
    {
        return augmentationList.Find((a) => a is T);
    }

    public bool HasAugmentation<T>() where T : Augmentation
    {
        return augmentationList.Find((a) => a is T) is not null;
    }

    public int GetAugmentationLevel<T>() where T : Augmentation
    {
        return augmentationList.Find((a) => a is T).level;
    }

    public int GetAugmentationLevel(string augName)
    {
        return augmentationList.Find((a) => string.Equals(a.GetType().Name, augName)).level;
    }
}
