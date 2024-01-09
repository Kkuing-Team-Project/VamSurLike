using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitArgs : EventArgs
{
    public Vector3 hitPoint { get; private set; }
    public Entity collided { get; private set; }

    public OnHitArgs(Vector3 hitPoint, Entity collided)
    {
        this.hitPoint = hitPoint;
        this.collided = collided;
    }
}
