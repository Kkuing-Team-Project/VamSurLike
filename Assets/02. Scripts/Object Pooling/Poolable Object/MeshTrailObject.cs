using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrailObject : MonoBehaviour, IPoolable
{
    public ObjectPool pool { get; set; }

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    Material currentMaterial;
    public void OnActivate()
    {
    }

    public void OnCreate()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshFilter = GetComponentInChildren<MeshFilter>();
    }

    public void ReturnObject()
    {
        pool.ReturnObject(gameObject, ObjectPool.ObjectType.MeshTrailObject);
    }

    public void SetMeshInfo(Mesh mesh, Material material)
    {
        meshFilter.mesh = mesh;
        currentMaterial = new Material(material);

        meshRenderer.material = currentMaterial;

        currentMaterial.SetFloat("_Alpha", 1f);
        StartCoroutine(AnimateMaterialFloat(currentMaterial, 0f, 0.1f, 0.05f));
    }
    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat("_Alpha");
        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;

            mat.SetFloat("_Alpha", valueToAnimate);
            Debug.Log(mat.GetFloat("_Alpha"));
            yield return new WaitForSeconds(refreshRate);
        }
        
        ReturnObject();
    }
}
