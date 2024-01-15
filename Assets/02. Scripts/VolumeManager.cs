using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : MonoBehaviour
{    
    Volume volum;
    Vignette vignette;
    LensDistortion lensDistortion;

    Coroutine hitEffectCoroutine;

    private void Awake()
    {
        volum = GetComponent<Volume>();
        volum.profile.TryGet(out vignette);
        volum.profile.TryGet(out lensDistortion);
    }

    public void StartHitEffect(float time)
    {
        if (hitEffectCoroutine != null)
        {
            StopCoroutine(hitEffectCoroutine);
        }
        hitEffectCoroutine = StartCoroutine(HitEffect(time));
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
}
