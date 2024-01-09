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


public delegate void AugmentationDelegate(Entity sender, EventArgs e);

public class Augmentation
{
    protected PlayableCtrl player { get; private set; }
    public AugmentationEventType eventType { get; private set; }
    public int level { get; private set; }

    public Augmentation(PlayableCtrl player, int level, AugmentationEventType eventType)
    {
        this.player = player;
        this.eventType = eventType;
        this.level = level;
    }

    public virtual void AugmentationEffect(Entity sender, EventArgs e)
    {

    }

    public void SetAugmentationLevel(int level) => this.level = level;
}

