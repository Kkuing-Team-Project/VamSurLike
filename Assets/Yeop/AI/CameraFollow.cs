using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void LateUpdate()
    {
        // 플레이어를 따라가는 카메라
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}