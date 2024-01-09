using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public abstract class PlayableCtrl : Entity
{
    public int level { get; private set; }
    public float exp { get; private set; }
    public float requireExp { get; private set; } = 10;

    public event AugmentationDelegate OnStartAugmentation;
    public event AugmentationDelegate OnUpdateAugmentation;
    public event AugmentationDelegate OnAttackPlayer;
    public event AugmentationDelegate OnBulletHit;

    private AugEventArgs defaultArgs;

    [Header("오브젝트 풀"), SerializeField]
    private ObjectPool bulletObjectPool;

    [Header("총알 갯수")]
    public int bulletNum;

    [Header("총알 간 각도")]
    public float bulletInterval;

    [Header("초당 회전 각도 값")]
    public float rotationAnglePerSecond;

    [Header("점멸 속도"), SerializeField]
    private float dashSpeed = 40f;

    [Header("점멸 이동 시간"), SerializeField]
    private float dashTime;
        

    // 이동 입력값
    private Vector3 inputVector;

    // 코루틴
    private Coroutine attackCor;
    private Coroutine dashCor;

    // 증강 리스트
    private List<Augmentation> augmentationList = new List<Augmentation>();

    // 임시 이펙트 오브젝트
    GameObject tempEffectObj;

    protected override void InitEntity()
    {
        base.InitEntity();
        stat.SetDefault(StatType.MOVE_SPEED, 3);
        defaultArgs = new AugEventArgs(transform, this);
        tempEffectObj = transform.Find("Effect Obj").gameObject;
    }

    void FixedUpdate()
    {
        if (dashCor == null)
        {
            rigid.velocity = inputVector.normalized * stat.Get(StatType.MOVE_SPEED);
        }
    }

    [ContextMenu("증강 추가 테스트")]
    public void AddAugmentationTest()
    {
        AddAugmentation(new DamageUp(1, AugmentationEventType.ON_START));
    }

    protected override void UpdateEntity()
    {
        Debug.Log(hp);
        OnUpdateAugmentation?.Invoke(this, defaultArgs);

        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.z = Input.GetAxisRaw("Vertical");

        // 공격 범위 내에 적이 있다면.
        if(GetNearestEnemy() != null && GetNearestEnemy().gameObject.activeSelf)
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
        var enemies = Physics.OverlapSphere(transform.position, stat.Get(StatType.ATTACK_DISTANCE), 1 << LayerMask.NameToLayer("ENEMY"));
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

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("ENEMY"));
        if (enemies.Length > 0 )
        {
            foreach(var enemy in enemies)
            {
                Entity target = enemy.GetComponent<Entity>();
                target.TakeDamage(this, 10);
                Vector3 knockbackDirection = (target.transform.position - transform.position).normalized;
                target.AddEffect(new Stun(1, 0.2f, this));
                target.rigid.AddForce(knockbackDirection * 20, ForceMode.Impulse);
            }
        }
        StopCoroutine("DisableEffect");
        StartCoroutine("DisableEffect");
    }
    private IEnumerator DisableEffect()
    {
        tempEffectObj.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        tempEffectObj.SetActive(false);
    }
     
    protected abstract void PlayerSkill();


    protected virtual void PlayerAttack(int bulletNum, float interval)
    {
        
        for (int i = 0; i < bulletNum; i++)
        {
            CreateBullet(50, transform.eulerAngles.y + (-interval * (bulletNum - 1) / 2 + i * interval));
        }
    }

    private IEnumerator AttackCoroutine()
    {
        WaitForSeconds attackDelay = new WaitForSeconds(1 / stat.Get(StatType.ATTACK_SPEED));
        while (true)
        {
            OnAttackPlayer?.Invoke(this, defaultArgs);
            PlayerAttack(bulletNum, bulletInterval);
            yield return attackDelay;
        }
    }

    //증강 추가 메소드
    public void AddAugmentation(Augmentation aug)
    {
        if (!HasAugmentation<Augmentation>())
        {
            switch (aug.eventType)
            {
                case AugmentationEventType.ON_START:
                    aug.AugmentationEffect(this, defaultArgs);
                    OnStartAugmentation += aug.AugmentationEffect;
                    break;
                case AugmentationEventType.ON_UPDATE:
                    OnUpdateAugmentation += aug.AugmentationEffect;
                    break;
                case AugmentationEventType.ON_ATTACK:
                    OnAttackPlayer += aug.AugmentationEffect;
                    break;
                case AugmentationEventType.ON_HIT:
                    OnBulletHit += aug.AugmentationEffect;
                    break;
                default:
                    break;
            }
            augmentationList.Add(aug);
        }
        else
        {
            GetAugmentation<Augmentation>().SetAugmentationLevel(GetAugmentationLevel<Augmentation>() + 1);
        }
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
            case AugmentationEventType.ON_START:
                OnStartAugmentation -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            case AugmentationEventType.ON_UPDATE:
                OnUpdateAugmentation -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            case AugmentationEventType.ON_ATTACK:
                OnUpdateAugmentation -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            case AugmentationEventType.ON_HIT:
                OnBulletHit -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            default:
                break;
        }
        augmentationList.Remove(del);
    }

    //증강 삭제(이름과 호출 타입 필요)
    public void DeleteAugmentation(string augName, AugmentationEventType type)
    {
        if (augmentationList.Count <= 0)
            return;

        Augmentation del = augmentationList.Find((a) => string.Equals(a.GetType().Name, augName));

        if (del == null)
            return;

        switch (type)
        {
            case AugmentationEventType.ON_START:
                OnStartAugmentation -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            case AugmentationEventType.ON_UPDATE:
                OnUpdateAugmentation -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            case AugmentationEventType.ON_ATTACK:
                OnUpdateAugmentation -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            case AugmentationEventType.ON_HIT:
                OnBulletHit -= new AugmentationDelegate(del.AugmentationEffect);
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

    public TempBullet CreateBullet(float speed, float rot)
    {
        TempBullet bullet = bulletObjectPool.Pop(ObjectPool.ObjectType.Bullet, transform.position).GetComponent<TempBullet>();

        bullet.player = this;
        bullet.transform.eulerAngles = new Vector3(0, rot, 0);
        bullet.rigid.velocity = speed * bullet.transform.forward;
        return bullet;
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

    public void InvokeEvent(AugmentationEventType type, Entity sender, AugEventArgs e)
    {
        switch (type)
        {
            case AugmentationEventType.ON_START:
                OnStartAugmentation?.Invoke(sender, e);
                break;
            case AugmentationEventType.ON_UPDATE:
                OnUpdateAugmentation?.Invoke(sender, e);
                break;
            case AugmentationEventType.ON_ATTACK:
                OnAttackPlayer?.Invoke(sender, e);
                break;
            case AugmentationEventType.ON_HIT:
                OnBulletHit?.Invoke(sender, e);
                break;
            default:
                break;
        }
    }
}
