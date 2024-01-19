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
        StartCoroutine(TextCor(2f));
    }

    public void OnCreate()
    {
        text = gameObject.GetComponentInChildren<Text>();
    }

    private IEnumerator TextCor(float time)
    {
        float elapsedTime = 0;
        float destroyDelay = 0.5f;
        float speed = 1;
        text.fontStyle = FontStyle.Bold;
        while (elapsedTime <  time)
        {
            yield return null;
            elapsedTime += Time.deltaTime * speed;
            if (elapsedTime > destroyDelay * time)
            {
                speed = 2;
                text.color = new Color(1, 1, 1, 1 - (elapsedTime - destroyDelay * time) / destroyDelay * time);
                text.rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, originPos + Vector3.up * (elapsedTime - destroyDelay * time));
            }
            else
            {
                text.rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, originPos);
            }
        }

        text.color = new Color(1, 1, 1, 1);
        ReturnObject();
    }

    public void ReturnObject()
    {
        pool?.ReturnObject(gameObject, ObjectPool.ObjectType.DamageText);
    }
}
