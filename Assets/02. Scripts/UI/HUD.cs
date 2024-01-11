using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace VamSureLikeSystem.MenuSystem
{
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
					// float ���� ����ġ = GameManager.instance.EXP;
					// float max����ġ = GameManager.instance.nextEXP[GameManager.instance.LEVEL];
					// mySlider.value = ���� ����ġ / max����ġ;
					break;
				case InfoType.LEVEL:
					// myText.text = string.Format("Lv.{0:F0}", GameManager.instance.LEVEL);
					break;
				case InfoType.KILL:
					// myText.text = string.Format("{0:F0}", GameManager.instance.KILL);
					break;
				case InfoType.TIME:
					// float �����ð� = GameManager.instance.�ִ�ð� - GameManager.instance.����ð�
					// int min(��) = Mathf.FloorToInt(�����ð� / 60);
					// int sec(��) = Mathf.FloorToInt(�����ð� % 60);
					// myText.text = string.Format("{0:D2}:{1:D2}", min,sec);
					break;
				case InfoType.HEALTH:
					// float ���� ü�� = GameManager.instance.HEALTH;
					// float maxü�� = GameManager.instance.maxHEALTH;
					// mySlider.value = ���� ü�� / maxü��;
					break;
				case InfoType.COIN:

					break;
			}
		}

		public void QuitGame()
        {
            Application.Quit(); // 게임 종료
        }
	}
}
