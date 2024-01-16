using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum SpiritState
{
    IDLE,       //휴식
    MOVE,       //이동
    OCCUPY,     //점령
}

public enum BlessType
{
    NONE,
    HEAL_MAG_UP,
    ATTACK_SPEED_UP,
    EXP_RANGE_UP,
}

[RequireComponent(typeof(NavMeshAgent))]
public class Spirit : Entity
{
    [SerializeField]
    private Spawner collapseZoneSpawner;
    private PlayableCtrl player;
    [HideInInspector]
    public CollapseZone collapseZone;
    public SpiritState spiritState { get; private set; }

    [Range(1, 10)]
    public float blessRange = 5f;
    public BlessType blessType { get; private set; }
    private NavMeshAgent nav;
    private Collider col;
    private MagicCircleEffect magicCircle;

    protected override void InitEntity()
    {
        base.InitEntity();
        if (player == null)
            player = FindObjectOfType<PlayableCtrl>();
        if(nav == null)
            nav = gameObject.GetComponent<NavMeshAgent>();
        col = gameObject.GetComponent<Collider>();
    }

    protected override void UpdateEntity()
    {
        if (collapseZone == null || collapseZone.gameObject.activeSelf == false)
        {
            if (collapseZoneSpawner) collapseZoneSpawner.stop = false;
            spiritState = SpiritState.IDLE;
        }
        else
        {
            if (collapseZoneSpawner) collapseZoneSpawner.stop = true;
            var zonePos = collapseZone.transform.position;
            zonePos.y = 0;
            var origin = transform.position;
            origin.y = 0;

            if ((origin - zonePos).magnitude >= 1f)
                spiritState = SpiritState.MOVE;
            else spiritState = SpiritState.OCCUPY;
        }
        SpiritBehavior();
        Bless(blessRange, blessType);
    }

    private void SpiritBehavior()
    {
        switch (spiritState)
        {
            case SpiritState.IDLE:
                col.enabled = false;
                break;
            case SpiritState.MOVE:
                nav.speed = stat.Get(StatType.MOVE_SPEED);
                nav.SetDestination(collapseZone.transform.position);
                col.enabled = false;
                break;
            case SpiritState.OCCUPY:
                col.enabled = true;
                break;
            default:
                break;
        }

        if (magicCircle != null) 
        {
            magicCircle.transform.position = transform.position;
        }
    }

    private void Bless(float radius, BlessType type)
    {

        if ((transform.position - player.transform.position).magnitude <= radius)
        {
            switch (type)
            {
                case BlessType.NONE:
                    break;
                case BlessType.HEAL_MAG_UP:
                    player.stat.Multiply(StatType.HEAL_MAG, 2.5f);
                    break;
                case BlessType.ATTACK_SPEED_UP:
                    player.stat.Multiply(StatType.ATTACK_SPEED, 1.5f);
                    break;
                case BlessType.EXP_RANGE_UP:
                    player.stat.Add(StatType.EXP_RANGE, 2);
                    break;
                default:
                    break;
            }
        }
    }

    public void SetBlessType()
    {
        if (magicCircle != null)
            magicCircle.ReturnObject();
        Heal(stat.Get(StatType.MAX_HP));
        magicCircle = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.MagicCircle, transform.position).GetComponent<MagicCircleEffect>();
        magicCircle.SetSize(Vector3.one * blessRange);
        while (true)
        {
            BlessType type = (BlessType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(BlessType)).Length);
            if (type != BlessType.NONE && type != blessType)
            {
                blessType = type;
                switch (blessType)
                {
                    case BlessType.NONE:
                        break;
                    case BlessType.HEAL_MAG_UP:
                        magicCircle.SetColor(Color.green);
                        break;
                    case BlessType.ATTACK_SPEED_UP:
                        magicCircle.SetColor(Color.yellow);
                        break;
                    case BlessType.EXP_RANGE_UP:
                        magicCircle.SetColor(Color.white);
                        break;
                    default:
                        break;
                }
                return;
            }
        }
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        if(spiritState != SpiritState.OCCUPY)
        {
            AddEffect(new Invincible(1, Time.deltaTime, this));
        }
    }
    protected override void OnEntityDied()
    {

    }

    private void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            switch(blessType) 
            {
                case BlessType.NONE:
                    Gizmos.color = Color.clear;
                    break;
                case BlessType.HEAL_MAG_UP:
                    Gizmos.color = Color.yellow;
                    break;
                case BlessType.ATTACK_SPEED_UP:
                    Gizmos.color = Color.red;
                    break;
                case BlessType.EXP_RANGE_UP:
                    Gizmos.color = Color.green;
                    break;
            }
            Gizmos.DrawWireSphere(transform.position, blessRange);
        }
    }
}
