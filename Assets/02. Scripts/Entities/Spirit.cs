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
    MOVE_SPEED_UP,
    ATTACK_SPEED_UP,
    EXP_RANGE_UP
}

[RequireComponent(typeof(NavMeshAgent))]
public class Spirit : Entity
{
    private PlayableCtrl player;
    public CollapseZone collapseZone;
    public SpiritState spiritState { get; private set; }

    [Range(1, 10)]
    public float blessRange = 5f;
    public BlessType blessType { get; private set; }
    private NavMeshAgent nav;

    protected override void InitEntity()
    {
        base.InitEntity();
        if (player == null)
            player = FindObjectOfType<PlayableCtrl>();
        if(nav == null)
            nav = gameObject.GetComponent<NavMeshAgent>();
    }

    protected override void UpdateEntity()
    {
        if(collapseZone == null)
        {
            spiritState = SpiritState.IDLE;
        }
        else
        {
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
                break;
            case SpiritState.MOVE:
                nav.speed = stat.Get(StatType.MOVE_SPEED);
                nav.SetDestination(collapseZone.transform.position);
                break;
            case SpiritState.OCCUPY:
                break;
            default:
                break;
        }
    }

    private void Bless(float range, BlessType type)
    {
        Heal(stat.Get(StatType.MAX_HP));
        if ((transform.position - player.transform.position).magnitude <= range)
        {
            switch (type)
            {
                case BlessType.NONE:
                    break;
                case BlessType.MOVE_SPEED_UP:
                    player.stat.Add(StatType.MOVE_SPEED, player.stat.Get(StatType.MOVE_SPEED) * 0.3f);
                    break;
                case BlessType.ATTACK_SPEED_UP:
                    player.stat.Add(StatType.ATTACK_SPEED, player.stat.Get(StatType.ATTACK_SPEED) * 0.3f);
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
        while(true)
        {
            BlessType type = (BlessType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(BlessType)).Length);
            if (type != BlessType.NONE && type != blessType)
            {
                blessType = type;
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
                case BlessType.MOVE_SPEED_UP:
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
