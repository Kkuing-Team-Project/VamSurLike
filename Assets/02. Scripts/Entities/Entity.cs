using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Entity : MonoBehaviour
{
    protected Stat stat;
    public float hp { get; protected set; }
    protected List<StatusEffect> statusEffects = new List<StatusEffect>();

    // Start is called before the first frame update
    void OnEnable()
    {
        InitEntity();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEntity();
    }

    protected virtual void InitEntity()
    {
        stat = new Stat();
        hp = stat.Get(StatType.MAX_HP);
    }

    protected virtual void UpdateEntity()
    {
        UpdateEffect();
    }

    private void LateUpdate()
    {
        stat.Update();
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
        hp -= dmg;
        if (hp <= 0)
            OnEntityDied();
    }

    public void Heal(float amount)
    {
        hp = Mathf.Clamp(hp + amount, 0, stat.Get(StatType.MAX_HP));
    }

    protected abstract void OnEntityDied();
}
