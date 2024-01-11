using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGaugeBar : MonoBehaviour
{
    public GaugeBar HpBar;
    public GaugeBar DashBar;

    PlayableCtrl player;

    private void Start()
    {
        player = GameManager.instance.player;
    }


}
