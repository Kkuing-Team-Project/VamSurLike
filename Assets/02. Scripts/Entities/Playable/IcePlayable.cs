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
    }

    protected override void PlayerSkill()
    {
        skillCor = StartCoroutine(SkillCor());
        rigid.velocity = Vector3.zero;
    }

    IEnumerator SkillCor()
    {
        animator.SetLayerWeight(1, 0);
        isAction = true;
        animator.SetTrigger("Skill");
        yield return new WaitForSeconds(15f);
        skillCor = null;
    }

    public void FreezeAround()
    {
        GameObject effect = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.FreezeCircle, transform.position);
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

    public void QuitSkill()
    {
        animator.SetLayerWeight(1, 1);
        isAction = false;
    }
}