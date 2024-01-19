using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritBar : MonoBehaviour
{
    [Header("Ã¼·Â¹Ù")]
    public GaugeBar hpBar;

    [Header("Å¸°Ù")]
    public Transform target;

    RectTransform rectTrf;

    private void Awake()
    {
        rectTrf = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        rectTrf.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);
    }
}
