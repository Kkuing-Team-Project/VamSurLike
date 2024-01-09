using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AugmentationEventType
{
    ON_START,       //증강 획득 시
    ON_UPDATE,      //업데이트마다
    ON_ATTACK,      //공격 시
    ON_HIT,         //적에게 닿았을 때
}


/// <summary>
/// Delegate for Augmentation
/// </summary>
/// <param name="sender">
/// sender: event invoker(default: player)
/// </param>
/// 
/// <param name="e">
/// 
/// e: Classes that inherit EventArgs
/// e.target: target that invoke event
/// eventType == ON_START (e.target: player, e.eventTr: player.transform)
/// eventType == ON_UPDATE: (e.target: player, e.eventTr: player.transform)
/// eventType == ON_ATTACK: (e.target: player, e.eventTr: player.transform)
/// eventType == ON_HIT: (e.target: damaged Enemy, e.eventTr: enemy.transform)
/// 
/// 
/// </param> 
/// 
public delegate void AugmentationDelegate(Entity sender, AugEventArgs e);

public class Augmentation
{
    public AugmentationEventType eventType { get; private set; }
    public int level { get; private set; }

    public Augmentation(int level, AugmentationEventType eventType)
    {
        this.eventType = eventType;
        this.level = level;
    }

    public virtual void AugmentationEffect(Entity sender, AugEventArgs e)
    {

    }

    public void SetAugmentationLevel(int level) => this.level = level;
}

