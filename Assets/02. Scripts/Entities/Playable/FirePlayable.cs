using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlayable : PlayableCtrl
{
    protected override void OnEntityDied()
    {
    }

    protected override void PlayerSkill()
    {
        StartCoroutine(FireAround());
    }

    IEnumerator FireAround()
    {
        Transform[] objects = new Transform[8];
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i] = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.FireBall, transform.position).transform;
        }

        float durationTimer = 0;
        float deg = 0;

        float circleR = 5f;
        float rotateSpeed = 360f;

        while (true)
        {
            durationTimer += Time.deltaTime;
            deg += rotateSpeed * Time.deltaTime;

            if (deg < 360)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    var rad = Mathf.Deg2Rad * (deg + (i * (360 / objects.Length)));
                    var x = circleR * Mathf.Sin(rad);
                    var z = circleR * Mathf.Cos(rad);
                    objects[i].SetPositionAndRotation(transform.position + new Vector3(x, 1f, z), Quaternion.Euler(0, 0, (deg + (i * (360 / objects.Length))) * -1));
                }
            }
            else
            {
                deg = 0;
            }

            if (durationTimer > 6f)
            {
                break;
            }
            yield return null;
        }

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<FireBall>().StopEffect();
        }
    }
}
