﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    MAX_HP,           
    MOVE_SPEED,          
    ATTACK_SPEED,
    DAMAGE,
    ATTACK_DISTANCE,
    SKILL_COOL_DOWN_MAG,   
    SKILL_FORCE_MAG,
    EXP_RANGE,
    HEAL_MAG,
}

public class Stat
{
    public Dictionary<StatType, float> defaultStat
        = new Dictionary<StatType, float>()
        {
            [StatType.MAX_HP] = 100f,
            [StatType.MOVE_SPEED] = 1f,
            [StatType.ATTACK_SPEED] = 2f,   //롤 공속식(초당 n회)
            [StatType.DAMAGE] = 10f,
            [StatType.ATTACK_DISTANCE] = 5,
            [StatType.SKILL_COOL_DOWN_MAG] = 1f,
            [StatType.SKILL_FORCE_MAG] = 1f,
            [StatType.EXP_RANGE] = 2f,
            [StatType.HEAL_MAG] = 1f,
        },
    addValue = new (),
    multipleValue = new(),
    currentValue = new Dictionary<StatType, float>()
    {
        [StatType.MAX_HP] = 100f,
        [StatType.MOVE_SPEED] = 1f,
        [StatType.ATTACK_SPEED] = 2f,
        [StatType.DAMAGE] = 10f,
        [StatType.ATTACK_DISTANCE] = 5,
        [StatType.SKILL_COOL_DOWN_MAG] = 1f,
        [StatType.SKILL_FORCE_MAG] = 1f,
        [StatType.EXP_RANGE] = 1f,
        [StatType.HEAL_MAG] = 1f
    };

    public Stat()
    {
        InitStat();
        UpdateStat();
    }

    public void Update()
    {
        UpdateStat();
        InitStat();
    }

    public void InitStat()
    {
        StatType[] statTypes = (StatType[])System.Enum.GetValues(typeof(StatType));

        foreach (var stat in statTypes)
        {
            addValue[stat] = 0;
            multipleValue[stat] = 1;
        }
    }

    public void UpdateStat()
    {
        StatType[] statTypes = (StatType[])System.Enum.GetValues(typeof(StatType));
        foreach (var type in statTypes)
        {
            currentValue[type] = (defaultStat[type] + addValue[type]) * multipleValue[type];
        }
    }

    public void SetDefault(StatType type, float defaultVal)
    {
        defaultStat[type] = defaultVal;
        Update();
    }

    public void Add(StatType type, float addition)
    {
        addValue[type] += addition;
    }

    public void Multiply(StatType type, float multiplier)
    {
        multipleValue[type] *= multiplier;
    }

    public float Get(StatType type)
    {
        return currentValue[type];
    }
}