using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GaugeBar : MonoBehaviour
{
    [System.Serializable]
    public struct GaugeBarInfo
    {
        public string type;
        public Image barImage;
    }

    public GaugeBarInfo[] barInformations;

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
