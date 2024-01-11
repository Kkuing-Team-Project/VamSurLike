using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GaugeBar : MonoBehaviour
{
    public Image barImage;

    public virtual void SetBarValue(float current, float max)
    {
        barImage.fillAmount = current / max;
    }
}
