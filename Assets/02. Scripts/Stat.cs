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
        },
    addValue = new Dictionary<StatType, float>()
    {
        [StatType.MAX_HP] = 0f,
        [StatType.MOVE_SPEED] = 0f,
        [StatType.ATTACK_SPEED] = 0f,
        [StatType.DAMAGE] = 0f,
        [StatType.ATTACK_DISTANCE] = 0,
        [StatType.SKILL_COOL_DOWN_MAG] = 0f,
        [StatType.SKILL_FORCE_MAG] = 0f,
        [StatType.EXP_RANGE] = 0f,
    },
    multipleValue = new Dictionary<StatType, float>()
    {
        [StatType.MAX_HP] = 1f,
        [StatType.MOVE_SPEED] = 1f,
        [StatType.ATTACK_SPEED] = 1f,
        [StatType.DAMAGE] = 1f,
        [StatType.ATTACK_DISTANCE] = 1f,
        [StatType.SKILL_COOL_DOWN_MAG] = 1f,
        [StatType.SKILL_FORCE_MAG] = 1f,
        [StatType.EXP_RANGE] = 1f
    },
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

    private void InitStat()
    {
        StatType[] statTypes = (StatType[])System.Enum.GetValues(typeof(StatType));

        foreach (var stat in statTypes)
        {
            addValue[stat] = 0;
            multipleValue[stat] = 1;
        }
    }

    private void UpdateStat()
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