using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGaugeBar : MonoBehaviour
{
    public GaugeBar HpBar;
    public GaugeBar DashBar;

    PlayableCtrl player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }


}
