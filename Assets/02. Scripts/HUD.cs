using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { EXP, LEVEL, KILL, TIME, HEALTH, COIN }
    public InfoType type;

    Text myText;
    Slider mySlider;

	void Awake()
	{
		myText = GetComponent<Text>();
		mySlider = GetComponent<Slider>();
	}

	void LateUpdate()
	{
		switch (type)
		{
			case InfoType.EXP:
				// float 현재 경험치 = GameManager.instance.EXP;
				// float max경험치 = GameManager.instance.nextEXP[GameManager.instance.LEVEL];
				// mySlider.value = 현재 경험치 / max경험치;
				break;
			case InfoType.LEVEL:
				// myText.text = string.Format("Lv.{0:F0}", GameManager.instance.LEVEL);
				break;
			case InfoType.KILL:
				// myText.text = string.Format("{0:F0}", GameManager.instance.KILL);
				break;
			case InfoType.TIME:
				// float 남은시간 = GameManager.instance.최대시간 - GameManager.instance.진행시간
				// int min(분) = Mathf.FloorToInt(남은시간 / 60);
				// int sec(초) = Mathf.FloorToInt(남은시간 % 60);
				// myText.text = string.Format("{0:D2}:{1:D2}", min,sec);
				break;
			case InfoType.HEALTH:
				// float 현재 체력 = GameManager.instance.HEALTH;
				// float max체력 = GameManager.instance.maxHEALTH;
				// mySlider.value = 현재 체력 / max체력;
				break;
			case InfoType.COIN:

				break;
		}
	}
}
