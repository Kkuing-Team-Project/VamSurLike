using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelegate : MonoBehaviour
{
    
    private Entity entity;

    private void Start()
    {
        entity = transform.parent.GetComponent<Entity>();
    }

    public void AnimationEvent(string animName)
    {
        entity.GetType().GetMethod(animName).Invoke(entity, null);
    }
}
