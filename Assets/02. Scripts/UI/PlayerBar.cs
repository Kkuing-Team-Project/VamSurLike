using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBar : MonoBehaviour
{
    public GaugeBar HpBar;
    public GaugeBar DashBar;

    RectTransform rectTrf;
    Transform target;

    private void Start()
    {
        rectTrf = GetComponent<RectTransform>();
        target = GameManager.instance.player.transform.Find("GaugeBarPosition");
    }

    private void FixedUpdate()
    {
        rectTrf.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);
    }
}
