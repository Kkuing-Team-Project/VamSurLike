using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GaugeBar : MonoBehaviour
{
    [System.Serializable]
    public struct GaugeBarInfo
    {
        public string type;
        public Image barImage;
        public bool isWolrdSpace;
    }

    public GaugeBarInfo[] barInformations;

    PlayableCtrl player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void Update()
    {
        for (int i =0; i < barInformations.Length; i++)
        {
            if (barInformations[i].isWolrdSpace)
            {
                barInformations[i].barImage.transform.LookAt(player.transform);
            }
        }
    }

    public void SetBarValue(string type, float current, float max)
    {
        foreach(var barInformation in barInformations)
        {
            if (barInformation.type.Equals(type))
            {
                barInformation.barImage.fillAmount = current / max;
            }
        }
    }
}
