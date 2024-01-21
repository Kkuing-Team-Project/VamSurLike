using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBar : MonoBehaviour
{
    public GaugeBar HpBar;
    public GaugeBar DashBar;

    RectTransform rectTrf;
    Camera targetCam;

    private void Start()
    {
        rectTrf = GetComponent<RectTransform>();
        targetCam = Camera.main;
    }

    private void LateUpdate()
    {
        rectTrf.rotation = targetCam.transform.rotation;
    }
}
