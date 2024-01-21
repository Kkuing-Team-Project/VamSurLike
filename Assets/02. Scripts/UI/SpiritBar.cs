using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritBar : MonoBehaviour
{
    [Header("Ã¼·Â¹Ù")]
    public GaugeBar hpBar;

    Camera targetCam;

    RectTransform rectTrf;

    private void Awake()
    {
        rectTrf = GetComponent<RectTransform>();
        targetCam = Camera.main;
    }

    private void FixedUpdate()
    {
        rectTrf.rotation = targetCam.transform.rotation;
    }
}
