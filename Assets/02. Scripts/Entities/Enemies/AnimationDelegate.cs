using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelegate : MonoBehaviour
{
    public Entity entity;
    

    public void AnimationEvent(string animName)
    {
        entity.GetType().GetMethod(animName).Invoke(entity, null);
    }
}
