using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGaugeBar : MonoBehaviour
{
    public GaugeBar HpBar { get; private set; }
    public GaugeBar DashBar { get; private set; }

    PlayableCtrl player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }


}
