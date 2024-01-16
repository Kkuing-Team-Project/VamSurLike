using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static bool IsTargetInSight(Transform target, Camera cam, float dis)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
        return /*screenPos.z > 0 && */screenPos.x > -dis && screenPos.x < Screen.width + dis && screenPos.y > -(dis * 2) && screenPos.y < Screen.height;
    }
}
