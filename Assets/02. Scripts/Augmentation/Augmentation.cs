using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AugmentationEventType
{
    ON_START,
    ON_UPDATE,
    ON_ATTACK,
}

public delegate void AugmentationDelegate(Entity sender, EventArgs e);

public class Augmentation
{
    protected PlayableCtrl player { get; private set; }
    public AugmentationEventType eventType { get; private set; }


    public Augmentation(PlayableCtrl player, AugmentationEventType eventType)
    {
        this.player = player;
        this.eventType = eventType;
    }

    public virtual void AugmentationEffect(Entity sender, EventArgs e)
    {

    }
}

