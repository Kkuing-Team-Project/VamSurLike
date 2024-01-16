using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionCulling : MonoBehaviour
{
    private readonly float distance = 150.0f;
    private void FixedUpdate()
    {
        Occlusion(transform);
    }

    public void Occlusion(Transform target)
    {
        bool isVisible = Util.IsTargetInSight(target, Camera.main, distance);
        for (int i = 0; i < target.childCount; i++)
        {
            target.GetChild(i).gameObject.SetActive(isVisible);
        }
    }
}
