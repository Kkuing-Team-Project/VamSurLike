using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour, IPoolable
{
    
    [HideInInspector]
    public Vector3 originPos;
    public ObjectPool pool { get; set; }
    public Text text;



    public void OnActivate()
    {
        StartCoroutine(TextCor(1f));
    }

    public void OnCreate()
    {
        text = gameObject.GetComponentInChildren<Text>();
    }

    private IEnumerator TextCor(float time)
    {
        float elapsedTime = 0;
        text.fontStyle = FontStyle.Bold;
        while (elapsedTime < time)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            text.rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, originPos + Vector3.up * elapsedTime);
            text.color = new Color(1, 1, 1, 1 - elapsedTime / time);
        }
        ReturnObject();
    }

    public void ReturnObject()
    {
        pool?.ReturnObject(gameObject, ObjectPool.ObjectType.DamageText);
    }
}
