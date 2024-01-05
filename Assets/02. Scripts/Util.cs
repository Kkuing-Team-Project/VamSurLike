using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{


    public static bool IsTargetInSight(Transform target, Transform origin, float degree)
    {
        Vector3 dir = (target.position - origin.position).normalized;

        float dot = Vector3.Dot(origin.forward, dir);
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return theta <= degree;
    }
}
