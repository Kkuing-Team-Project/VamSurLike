using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : Singleton<VolumeManager>
{
    [Header("바람 원소 스킬 시 화면 색상"), SerializeField]
    Color windSkillEffectColor;
    Volume volume;
    Vignette vignette;
    public Vignette Vignette => vignette;
    LensDistortion lensDistortion;
    MotionBlur motionBlur;

    Coroutine hitEffectCoroutine;

    public override void Awake()
    {
        base.Awake();
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out lensDistortion);
        volume.profile.TryGet(out motionBlur);

    }

    public void StartHitEffect(float time)
    {
        if (hitEffectCoroutine != null)
        {
            StopCoroutine(hitEffectCoroutine);
        }
        hitEffectCoroutine = StartCoroutine(HitEffect(time));
    }

    public void StartWindSkillEffect(float time)
    {
        if (hitEffectCoroutine != null)
        {
            StopCoroutine(hitEffectCoroutine);
            hitEffectCoroutine = null;
        }
        StartCoroutine(WindSkillEffect(time));
    }

    public void StartDeathEffect(float time)
    {
        if(hitEffectCoroutine != null)
        {
            StopCoroutine(hitEffectCoroutine);
            hitEffectCoroutine = null;
        }
        StartCoroutine(DeathEffect(time));
    }

    public void SetActiveMotionBlur(bool active)
    {
        motionBlur.active = active;
    }

    IEnumerator HitEffect(float time)
    {
        float timer = time;
        vignette.intensity.value = 0.4f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(0f, 0.4f, timer / time);
            yield return null;
        }
        vignette.intensity.value = 0f;
        hitEffectCoroutine = null;
    }

    IEnumerator WindSkillEffect(float time)
    {
        float timer = time;
        float changeValue = 0f;
        float changeValueRange = 0.05f;
        float startValue = 0.3f;
        vignette.color.value = windSkillEffectColor;
        vignette.intensity.value = startValue;
        while (timer > 0)
        {
            changeValue = Mathf.Sin(timer * 5f) * changeValueRange;
            vignette.intensity.value = startValue + changeValue;

            timer -= Time.deltaTime; 

            yield return null;
        }
        vignette.intensity.value = 0f;
        vignette.color.value = Color.black;
    }

    IEnumerator DeathEffect(float time)
    {
        vignette.color.value = Color.red;
        vignette.intensity.value = 0;
        for (float elapsedTime = 0; elapsedTime < time - 0.1f; elapsedTime += Time.deltaTime) 
        {
            vignette.intensity.value = Mathf.Lerp(0f, 0.5f, elapsedTime / time);
            yield return null;
        }
        vignette.intensity.value = 0.5f;
        for (float elapsedTime = 0; elapsedTime < 0.1f; elapsedTime += Time.deltaTime)
        {
            vignette.intensity.value = Mathf.Lerp(0.5f, 0f, elapsedTime / time);
            yield return null;
        }
        vignette.intensity.value = 0f;
    }
}
