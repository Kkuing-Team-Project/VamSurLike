using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    NONE,
    P_FIRE,
    P_ICE,
    P_WIND,
    E_StoneHead,
    E_Prion,
    E_Servant,
    E_DarkArcher,
    Soul,
    B_Giant,
}

[RequireComponent(typeof(Rigidbody))]
public abstract class Entity : MonoBehaviour
{
    public EntityType entityType;
    public Stat stat;
    public float hp { get; protected set; }
    protected List<StatusEffect> statusEffects = new List<StatusEffect>();
    protected Animator animator;
    
    [HideInInspector]
    public Rigidbody rigid;
    
    public Material[] originMaterials { get; protected set; }
    public Renderer meshRenderer { get; protected set; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        meshRenderer = GetComponentInChildren<Renderer>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    void Start()
    {
        InitEntity();
    }

    void Update()
    {
        UpdateEffect();
        if (!HasEffect<Stun>())
            UpdateEntity();
    }

    private void LateUpdate()
    {
        stat.Update();
    }

    protected virtual void InitEntity()
    {
        stat = new Stat();
        SetEntityStat(entityType);
        hp = stat.Get(StatType.MAX_HP);
        statusEffects.Clear();
    }

    protected abstract void UpdateEntity();

    protected virtual void UpdateEffect()
    {
        if (statusEffects.Count <= 0)
            return;

        List<StatusEffect> delEffect = new List<StatusEffect>();
        foreach (var eff in statusEffects)
        {
            eff.OnUpdate(this);
            eff.duration -= Time.deltaTime;
            if (eff.duration <= 0)
            {
                eff.OnFinish(this);
                delEffect.Add(eff);
            }
        }

        statusEffects.RemoveAll(eff => delEffect.Contains(eff));
    }

    public void AddEffect(StatusEffect effect)
    {
        statusEffects.Add(effect);
        effect.OnStart(this);
    }

    public bool HasEffect<T>() where T : StatusEffect
    {
        return !(statusEffects.Find(n => n is T) is null);
    }

    /// <summary>
    /// ��ƼƼ���� ����� �ִ�
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="dmg"></param>
    public void TakeDamage(Entity caster, float dmg)
    {
        OnTakeDamage(caster, dmg);

        DamageEffect damageEffect = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.DamageText, transform.position).GetComponent<DamageEffect>();

        damageEffect.originPos = transform.position;
        if (HasEffect<Invincible>() == false)
        {
            damageEffect.text.text = Mathf.Round(dmg).ToString();
            hp -= dmg;
        }
        else
        {
            damageEffect.text.text = "Miss";
        }

        if (hp <= 0)
            OnEntityDied();
    }

    public void Heal(float amount)
    {
        hp = Mathf.Clamp(hp + amount, 0, stat.Get(StatType.MAX_HP));
    }

    protected abstract void OnTakeDamage(Entity caster, float dmg);

    protected abstract void OnEntityDied();

    protected bool IsAnimationClipPlaying(string name, int layerIdx)
    {
        return animator.GetCurrentAnimatorStateInfo(layerIdx).IsName(name) && animator.GetCurrentAnimatorStateInfo(layerIdx).normalizedTime < 1f;
    }

    /// <summary>
    /// Method that initialize stat
    /// This method refer to GameManager, Resource/Data/StatTable.csv
    /// </summary>
    public void SetEntityStat(EntityType type)
    {
        int idx = -1;
        for (int i = 0; i < GameManager.instance.statTable.Count; i++)
        {
            if (type.ToString().Equals(GameManager.instance.statTable[i]["CHARACTER_ID"].ToString()))
            {
                idx = i;
                break;
            }
        }

        if (idx == -1)
        {
            Debug.LogError($"{gameObject.name} : Value not found!");
            return;
        }

        var keys = new List<string>(GameManager.instance.statTable[idx].Keys);
        for (int i = 1; i < GameManager.instance.statTable[0].Count; i++)
        {
            if (keys[i].ToString().Equals("ATTACK_DISTANCE") && gameObject.GetComponent<CapsuleCollider>().radius + 1 > float.Parse(GameManager.instance.statTable[idx][keys[i]].ToString()))
            {
                stat.SetDefault(Enum.Parse<StatType>(keys[i]), gameObject.GetComponent<CapsuleCollider>().radius + 1);
            }
            else
            {
                stat.SetDefault(Enum.Parse<StatType>(keys[i]), float.Parse(GameManager.instance.statTable[idx][keys[i]].ToString()));
            }
        }
    }

    public void ResetMaterial()
    {
        meshRenderer.materials = originMaterials;
    }

    public void SetAnimationPlaying(bool active)
    {
        if (active == false)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }
    }
}
