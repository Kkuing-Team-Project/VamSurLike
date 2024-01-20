using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlayable : PlayableCtrl
{
    protected override void OnEntityDied()
    {
        SoundManager.Instance.PlaySound("Sound_EF_CH_Death");
    }

    protected override void PlayerSkill()
    {
        rigid.velocity = Vector3.zero;
        animator.SetTrigger("Skill");
        SoundManager.Instance.PlaySound("Sound_EF_CH_Skill_Fire");
        animator.SetLayerWeight(1, 0f);
        animator.SetLayerWeight(2, 1f);
    }

    public void MakeFire()
    {
        skillCor = StartCoroutine(FireAround());
    }

    public void QuitSkillAnimation()
    {
        animator.SetLayerWeight(1, 1f);
        animator.SetLayerWeight(2, 0f);
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

        float circleR = 1f;
        float rotateSpeed = 360f;

        while (true)
        {
            durationTimer += Time.deltaTime;
            deg += rotateSpeed * Time.deltaTime;
            circleR = Mathf.Lerp(1f, 5f, durationTimer / 6f);
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
                durationTimer = 0;
                circleR = 5f;
                break;
            }
            yield return null;
        }

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<FireBall>().StopEffect();
            
        }
        // --------------------------------------------------------------------------------------------- Effect After skill
        while (durationTimer <= 1f)
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
            yield return null;
        }
        StartCoroutine(hud.CoolTimeUICor(GetSkillCoolTime()));
        yield return new WaitForSeconds(GetSkillCoolTime());
        skillCor = null;
    }

    protected override float GetSkillCoolTime()
    {
        return 15f;
    }
}
