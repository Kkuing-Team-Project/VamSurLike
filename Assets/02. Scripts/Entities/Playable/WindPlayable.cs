using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WindPlayable : PlayableCtrl
{
    [Header("�ܻ� ���� ����"), SerializeField]
    float meshRefreshRate = 0.1f;
    [Header("�ܻ� ��Ƽ����"), SerializeField]
    Material trailMaterial;

    SkinnedMeshRenderer[] skinnedMeshRenderers;

    
    IEnumerator ActivateTrail(float timeActive)
    {
        animator.SetBool("Skill", true);
        
        stat.Multiply(StatType.MOVE_SPEED, 1.5f);
        VolumeManager.Instance.SetActiveMotionBlur(true);
        VolumeManager.Instance.StartWindSkillEffect(timeActive);
        statusEffects.Add(new Invincible(1, timeActive, this));

        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
            {
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                SoundManager.Instance.PlaySound("Sound_EF_CH_Skill_Wind");
            }

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                MeshTrailObject meshTrailObj = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.MeshTrailObject, transform.position).GetComponent<MeshTrailObject>();
                meshTrailObj.transform.rotation = transform.rotation;

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                meshTrailObj.SetMeshInfo(mesh, trailMaterial);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }
        animator.SetBool("Skill", false);
        stat.Multiply(StatType.MOVE_SPEED, 1f);
        VolumeManager.Instance.SetActiveMotionBlur(false);
        StartCoroutine(hud.CoolTimeUICor(GetSkillCoolTime()));
        yield return new WaitForSeconds(GetSkillCoolTime());

        skillCor = null;
    }

    protected override void OnEntityDied()
    {
        SoundManager.Instance.PlaySound("Sound_EF_CH_Death");
    }

    protected override void PlayerSkill()
    {
        skillCor = StartCoroutine(ActivateTrail(6f));
    }

    protected override float GetSkillCoolTime()
    {
        return 15f;
    }
}
