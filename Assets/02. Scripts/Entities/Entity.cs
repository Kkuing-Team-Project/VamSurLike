using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Entity : MonoBehaviour
{
    protected Stat stat;
    public float Hp { get; protected set; }
    protected List<StatusEffect> statusEffects = new List<StatusEffect>();
    protected Rigidbody rigid;

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
        Hp = stat.Get(StatType.MAX_HP);

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

    public void TakeDamage(float dmg)
    {
        Hp -= dmg;
        if (Hp <= 0)
            OnEntityDied();
    }

    public void Heal(float amount)
    {
        Hp = Mathf.Clamp(Hp + amount, 0, stat.Get(StatType.MAX_HP));
    }

    protected abstract void OnEntityDied();
}
