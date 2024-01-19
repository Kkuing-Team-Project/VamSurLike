using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    [Header("체력바")]
    public GaugeBar hpBar;

    [Header("타겟")]
    public Transform target;

    [Header("페이드 아웃 속도"), SerializeField]
    float fadeSpeed = 2f;

    // 오브젝트에 속한 모든 이미지
    Image[] images;

    RectTransform rectTrf;

    // 페이드 아웃 효과 코루틴
    Coroutine fadeEffectCoroutine;

    private void Awake()
    {
        images = GetComponentsInChildren<Image>();
        rectTrf = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        rectTrf.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);
    }

    public void AllocateTarget(Transform target)
    {
        this.target = target;
        ActivateGaugeBar();
    }

    public void ActivateGaugeBar()
    {
        if (fadeEffectCoroutine != null)
        {
            StopCoroutine(fadeEffectCoroutine);
        }
        fadeEffectCoroutine = StartCoroutine(FadeGaugeBarEffect());
    }

    IEnumerator FadeGaugeBarEffect()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = Color.white;
        }
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < images.Length; i++)
        {
            images[i].CrossFadeColor(Color.clear, fadeSpeed, false, true);
        }
    }
}