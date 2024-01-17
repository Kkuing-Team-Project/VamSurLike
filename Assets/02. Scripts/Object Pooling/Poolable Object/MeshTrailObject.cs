using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrailObject : MonoBehaviour, IPoolable
{
    public ObjectPool pool { get; set; }

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    public void OnActivate()
    {
        meshRenderer.material.SetFloat("_Alpha", 1f);
        StartCoroutine(AnimateMaterialFloat(meshRenderer.material, 0f, 0.2f, 0.1f));
    }

    public void OnCreate()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }

    public void ReturnObject()
    {
        pool.ReturnObject(gameObject, ObjectPool.ObjectType.MeshTrailObject);
    }

    public void SetMeshInfo(Mesh mesh, Material material)
    {
        meshFilter.mesh = mesh;
        meshRenderer.material = material;
    }
    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat("_Alpha");

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat("_Alpha", valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }

        ReturnObject();
    }
}
