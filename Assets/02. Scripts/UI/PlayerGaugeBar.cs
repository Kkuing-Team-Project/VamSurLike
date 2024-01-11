using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGaugeBar : MonoBehaviour
{
    public GaugeBar HpBar;
    public GaugeBar DashBar;

    RectTransform rectTrf;
    PlayableCtrl player;

    private void Start()
    {
        rectTrf = GetComponent<RectTransform>();
        player = GameManager.instance.player;
    }

    private void FixedUpdate()
    {
        rectTrf.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position + Vector3.up * 3f);
    }
}
