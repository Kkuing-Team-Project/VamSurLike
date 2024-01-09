using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugEventArgs : EventArgs
{
    public Transform eventTr { get; private set; }
    public Entity target { get; private set; }

    public AugEventArgs(Transform eventTr, Entity target)
    {
        this.eventTr = eventTr;
        this.target = target;
    }
}
