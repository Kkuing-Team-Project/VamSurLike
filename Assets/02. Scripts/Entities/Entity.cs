using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Entity : MonoBehaviour
{
    public Stat stat;
    public float hp { get; protected set; }
    protected List<StatusEffect> statusEffects = new List<StatusEffect>();
    protected Animator animator;
    [HideInInspector]
    public Rigidbody rigid;
    void OnEnable()
    {
        InitEntity(); 
    }

    void Update()
    {
        UpdateEntity();
    }

    private void LateUpdate()
    {
        stat.Update();
    }

    protected virtual void InitEntity()
    {
        stat = new Stat();
        hp = stat.Get(StatType.MAX_HP);
        animator = gameObject.GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    protected virtual void UpdateEntity()
    {
        UpdateEffect();
    }

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

    /// <summary>
    /// 엔티티에게 대미지 주는
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="dmg"></param>
    public void TakeDamage(Entity caster, float dmg)
    {
        hp -= dmg;

        OnTakeDamage(caster, dmg);

        if (hp <= 0)
            OnEntityDied();
    }

    public void Heal(float amount)
    {
        hp = Mathf.Clamp(hp + amount, 0, stat.Get(StatType.MAX_HP));
    }

    protected abstract void OnTakeDamage(Entity caster, float dmg);

    protected abstract void OnEntityDied();

    //현재 해당 애니메이션 호출 중인지
    //name: 애니메이터 내 클립의 이름
    //layerIdx: 호출할 애니메이션의 레이어 (기본 0)
    protected bool IsAnimationClipPlaying(string name, int layerIdx)
    {
        return animator.GetCurrentAnimatorStateInfo(layerIdx).IsName(name) && animator.GetCurrentAnimatorStateInfo(layerIdx).normalizedTime < 1f;
    }

}
