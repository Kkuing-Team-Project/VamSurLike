using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using TMPro;
using TreeEditor;
using UnityEditor.SceneManagement;
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
    public event AugmentationDelegate OnTakeDamageAugmentation;
    public event AugmentationDelegate OnSpawnEnemy;
    public event AugmentationDelegate OnUpdateEnemy;

    public HUD hud;

    public string testAugName;

    private AugEventArgs defaultArgs;

    [Header("게이지 바"), SerializeField]
    PlayerGaugeBar gaugeBar;


    [Header("총알 갯수")]
    public int bulletNum;

    [Header("총알 간 각도")]
    public float bulletInterval;

    [Header("초당 회전 각도 값")]
    public float rotationAnglePerSecond = 270f;

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

    bool canMove = true;

    // Components applied to the player object.
    ObjectPool objectPool;
    CinemachineImpulseSource cameraShakeSource;

    [Header("테스트용 임시 값들")]
    public bool isTest = false;
    [Tooltip("경험치 획득 범위"), SerializeField]
    float tempExpRange = 7f;
    [Tooltip("캐릭터 이동 속도"), SerializeField]
    float tempMoveSpeed = 5f;
    [Tooltip("공격 사거리"), SerializeField]
    float tempAttackRange = 12f;
    [Tooltip("공격 속도"), SerializeField]
    float tempAttackSpeed = 2.5f;
    protected override void InitEntity()
    {
        base.InitEntity();

        if (isTest)
        {
           stat.SetDefault(StatType.EXP_RANGE, tempExpRange);
           stat.SetDefault(StatType.MOVE_SPEED, tempMoveSpeed);
           stat.SetDefault(StatType.ATTACK_DISTANCE, tempAttackRange);
           stat.SetDefault(StatType.ATTACK_SPEED, tempAttackSpeed);
        }
        else
        {
           stat.SetDefault(StatType.MOVE_SPEED, 3);
        }


        defaultArgs = new AugEventArgs(transform, this);

        animator = GetComponent<Animator>();
        requireExp = int.Parse(GameManager.instance.levelTable[0]["NEED_EXP"].ToString());
        objectPool = FindObjectOfType<ObjectPool>();
        cameraShakeSource = GetComponent<CinemachineImpulseSource>();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rigid.velocity = inputVector.normalized * stat.Get(StatType.MOVE_SPEED);
        }
    }

    [ContextMenu("증강 추가 테스트")]
    public void AddAugmentationTest()
    {
        Type type = Type.GetType(testAugName);
        if (type == null)
        {
            Debug.LogError("Augmentation Not Found!");
            return;
        }
        Augmentation aug = null;
        if (HasAugmentation(testAugName) == false)
        {
            aug = Activator.CreateInstance(type, 1, GameManager.instance.GetAugMaxLevel(testAugName)) as Augmentation;
        }
        else
        {
            aug = Activator.CreateInstance(type, GetAugmentationLevel(testAugName), GameManager.instance.GetAugMaxLevel(testAugName)) as Augmentation;
        }
        AddAugmentation(aug);
    }

    protected override void UpdateEntity()
    {
        OnUpdateAugmentation?.Invoke(this, defaultArgs);

        #region Move Player
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.z = Input.GetAxis("Vertical");
        if (inputVector.magnitude > 0)
        {
            animator.SetBool("IsMove", true);

            animator.SetFloat("InputX", transform.InverseTransformVector(inputVector).x);
            animator.SetFloat("InputZ", transform.InverseTransformVector(inputVector).z);

            animator.speed = rigid.velocity.magnitude / 6f;     // Code to set animation speed based on movement speed
        }
        else
        {
            animator.SetBool("IsMove", false);
        }
        #endregion

        #region Check Experience Gem around player
        Collider[] experienceGems = Physics.OverlapSphere(transform.position, stat.Get(StatType.EXP_RANGE), LayerMask.GetMask("EXP"));
        if (experienceGems.LongLength > 0)
        {
            for (int i = 0; i < experienceGems.Length; i++)
            {
                experienceGems[i].GetComponent<ExperienceGem>().PullToPlayer(this);
            }
        }
        #endregion

        #region Check Enmey around player And Attack, Rotate
        Vector3 targetPosition = Vector3.zero;

        // 공격 범위 내에 적이 있다면.
        if (GetNearestEnemy() != null && GetNearestEnemy().gameObject.activeSelf)
        {
            targetPosition = GetNearestEnemy().transform.position;

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

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                targetPosition = hit.point;
            }
        }

        targetPosition.y = transform.position.y;
        Vector3 targetDirection = (targetPosition - transform.position).normalized;
        Quaternion nextRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), rotationAnglePerSecond * Time.deltaTime);  // 다음 프레임에 적용할 회전값

        transform.rotation = nextRotation;
        #endregion


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
        var enemies = Physics.OverlapSphere(transform.position, stat.Get(StatType.ATTACK_DISTANCE), 1 << LayerMask.NameToLayer("ENEMY") | 1 << LayerMask.NameToLayer("BOSS"));
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

    #region Dash Method
    protected IEnumerator DashCor()
    {
        canMove = false;
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

        canMove = true;

        gaugeBar.DashBar.SetBarValue(0, 5f);

        float cooltimeTimer = 0f;
        while (true)
        {
            cooltimeTimer += Time.deltaTime;
            gaugeBar.DashBar.SetBarValue(cooltimeTimer, 5f);

            yield return null;
            if (cooltimeTimer >= 5f)
            {
                gaugeBar.DashBar.SetBarValue(5f, 5f);
                break;
            }
        }

        dashCor = null;
    }
    #endregion

    #region Take Damage Method
    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        OnTakeDamageAugmentation?.Invoke(this, defaultArgs);

        gaugeBar.HpBar.SetBarValue(hp, stat.Get(StatType.MAX_HP));

        Collider[] enemies = Physics.OverlapSphere(transform.position, 3f, LayerMask.GetMask("ENEMY"));
        if (enemies.Length > 0)
        {
            foreach (var enemy in enemies)
            {
                Entity target = enemy.GetComponent<Entity>();
                target.TakeDamage(this, 10f);
                Vector3 knockbackDirection = (target.transform.position - transform.position).normalized;
                target.AddEffect(new Stun(1, 0.2f, this));
                target.rigid.AddForce(knockbackDirection * 20, ForceMode.Impulse);
            }
        }
        cameraShakeSource.GenerateImpulse();


        objectPool.GetObject(ObjectPool.ObjectType.HitParticle, transform.position + Vector3.up);
    }
    #endregion


    protected abstract void PlayerSkill();

    #region Attack
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
    protected virtual void PlayerAttack(int bulletNum, float interval)
    {
        for (int i = 0; i < bulletNum; i++)
        {
            CreateBullet(50, transform.eulerAngles.y + (-interval * (bulletNum - 1) / 2 + i * interval));
        }
    }

    #endregion

    #region Augmentation Method
    //증강 추가 메소드
    public void AddAugmentation(Augmentation aug)
    {
        Debug.Log($"{aug.GetType().Name} : {!HasAugmentation(aug.GetType().Name)}");
        if (!HasAugmentation(aug.GetType().Name))
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
                case AugmentationEventType.ON_DAMAGE:
                    OnTakeDamageAugmentation += aug.AugmentationEffect;
                    break;
                case AugmentationEventType.ON_SPAWN_ENEMY:
                    OnSpawnEnemy += aug.AugmentationEffect;
                    break;
                case AugmentationEventType.ON_UPDATE_ENEMY:
                    OnUpdateAugmentation += aug.AugmentationEffect;
                    break;
                default:
                    break;
            }
            augmentationList.Add(aug);
        }
        else
        {
            if(aug.eventType == AugmentationEventType.ON_START)
            {
                int level = GetAugmentationLevel(aug.GetType().Name) + 1;
                aug.SetAugmentationLevel(level);
                aug.AugmentationEffect(this, defaultArgs);
            }
            else
            {
                GetAugmentation(aug.GetType().Name).SetAugmentationLevel(GetAugmentationLevel(aug.GetType().Name) + 1);
            }
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
            case AugmentationEventType.ON_DAMAGE:
                OnTakeDamageAugmentation -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            case AugmentationEventType.ON_SPAWN_ENEMY:
                OnSpawnEnemy -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            case AugmentationEventType.ON_UPDATE_ENEMY:
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
            case AugmentationEventType.ON_DAMAGE:
                OnTakeDamageAugmentation -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            case AugmentationEventType.ON_SPAWN_ENEMY:
                OnSpawnEnemy -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            case AugmentationEventType.ON_UPDATE_ENEMY:
                OnUpdateAugmentation -= new AugmentationDelegate(del.AugmentationEffect);
                break;
            default:
                break;
        }
        augmentationList.Remove(del);
    }
    
    public void AddExp(float val)
    {
        if (level >= GameManager.instance.levelTable.Count - 1)
        {
            return;
        }
        else
        {

            exp += val;
            if(exp >= requireExp)
            {
                exp = exp - requireExp;
                level++;
                requireExp = int.Parse(GameManager.instance.levelTable[level]["NEED_EXP"].ToString());
                if(isTest == false)
                {
                    Time.timeScale = 0;
                    hud.augPanel.SetActive(true);
                    hud.SetAugmentation();
                }
            }
        }
    }

    public TempBullet CreateBullet(float speed, float rot)
    {
        TempBullet bullet = objectPool.GetObject(ObjectPool.ObjectType.Bullet, transform.position + Vector3.up).GetComponent<TempBullet>();

        bullet.player = this;
        bullet.transform.eulerAngles = new Vector3(0, rot, 0);
        bullet.rigid.velocity = speed * bullet.transform.forward;
        return bullet;
    }

    public Augmentation GetAugmentation<T>() where T : Augmentation
    {
        return augmentationList.Find((a) => a is T);
    }

    public Augmentation GetAugmentation(string augName)
    {
        return augmentationList.Find((a) => (a.GetType().Name == augName));
    }

    public bool HasAugmentation<T>() where T : Augmentation
    {
        return augmentationList.Find((a) => a is T) is not null;
    }

    public bool HasAugmentation(string augName)
    {
        return augmentationList.Find((a) => a.GetType().Name == augName) is not null;
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
            case AugmentationEventType.ON_DAMAGE:
                OnTakeDamageAugmentation?.Invoke(sender, e);
                break;
            case AugmentationEventType.ON_SPAWN_ENEMY:
                OnSpawnEnemy?.Invoke(sender, e);
                break;
            case AugmentationEventType.ON_UPDATE_ENEMY:
                OnUpdateAugmentation?.Invoke(sender, e);
                break;
            default:
                break;
        }
    }
    #endregion
}
