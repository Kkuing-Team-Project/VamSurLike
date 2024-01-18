using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WindPlayable : PlayableCtrl
{
    [Header("잔상 생성 간격"), SerializeField]
    float meshRefreshRate = 0.1f;
    [Header("잔상 머티리얼"), SerializeField]
    Material trailMaterial;

    bool isTrailActive = false;
    SkinnedMeshRenderer[] skinnedMeshRenderers;


    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
            {
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            }

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                MeshTrailObject meshTrailObj = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.MeshTrailObject, transform.position).GetComponent<MeshTrailObject>();
                meshTrailObj.transform.rotation = Quaternion.Euler(-90f, transform.rotation.y, 0f);

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                meshTrailObj.SetMeshInfo(mesh, trailMaterial);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        yield return new WaitForSeconds(15f);

        skillCor = null;
    }

    

    protected override void OnEntityDied()
    {
    }

    protected override void PlayerSkill()
    {
        skillCor = StartCoroutine(ActivateTrail(6f));
    }
}
