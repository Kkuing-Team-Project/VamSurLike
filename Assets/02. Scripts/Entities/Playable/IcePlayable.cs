using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IcePlayable : PlayableCtrl
{
    [SerializeField] Material freezeMaterial;
    protected override void OnEntityDied()
    {
        SoundManager.Instance.PlayOneShot("Sound_EF_CH_Death");
    }

    protected override void PlayerSkill()
    {
        skillCor = StartCoroutine(SkillCor());
        rigid.velocity = Vector3.zero;
    }

    private IEnumerator SkillCor()
    {
        animator.SetLayerWeight(1, 0);
        isAction = true;
        animator.SetTrigger("Skill");
        StartCoroutine(hud.CoolTimeUICor(GetSkillCoolTime()));
        yield return new WaitForSeconds(GetSkillCoolTime());
        skillCor = null;
    }

    public void FreezeAround()
    {
        GameObject effect = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.FreezeCircle, transform.position);
        SoundManager.Instance.PlayOneShot("Sound_EF_CH_Skill_Ice");
        effect.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        Collider[] enemies = Physics.OverlapSphere(transform.position, 10f, 1 << LayerMask.NameToLayer("ENEMY") | 1 << LayerMask.NameToLayer("BOSS"));
        if (enemies.Length > 0)
        {
            foreach (var enemy in enemies)
            {
                Entity target = enemy.GetComponent<Entity>();
                target.AddEffect(new Freeze(freezeMaterial, 1, 6f, target));
                target.AddEffect(new Stun(1, 6f, target));
            }
        }
    }

    public void QuitSkillAnimation()
    {
        animator.SetLayerWeight(1, 1);
        isAction = false;
    }

    protected override float GetSkillCoolTime()
    {
        return 15f;
    }
}