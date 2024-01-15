using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class HUD : MonoBehaviour
{
	public Text timeText;
	public Text killCountText;
	public Text levelText;
	public Slider expSlider;
	public GameObject augImg;
	public ScrollRect augScroll;
	public GameObject statImg;
	public Text statText;

	public GameObject augPanel;
	public Button[] augButtons;
	public Text[] augNameTexts;

	public PlayerGaugeBar playerGaugeBar;

	void LateUpdate()
	{
		UIUpdate();
	}

	private void UIUpdate()
	{
		if (GameManager.instance.player == null)
			return;

		int sceneStartTime = (int)Time.timeSinceLevelLoad;

		timeText.text = string.Format("{0:D2} : {1:D2}", sceneStartTime / 60, sceneStartTime % 60);

		killCountText.text = GameManager.instance.killCount.ToString();


		if (GameManager.instance.player.level >= GameManager.instance.levelTable.Count - 1)
		{
			levelText.text = "Max";
			expSlider.value = 1;
		}
		else
		{
			expSlider.value = GameManager.instance.player.exp / GameManager.instance.player.requireExp;
			levelText.text = $"Lv. {(GameManager.instance.player.level + 1).ToString()}";
		}

		if (Input.GetKey(KeyCode.C))
		{
			statImg.SetActive(true);
			statText.text = string.Format("MaxHP: {0:D2}\nDmg: {1:D2}\nAtkSpd: {2:D2}\nAtkDist: {3:D2}\nMvSpd: {4:D2}\nExpRge: {5:D2}",
				(int)GameManager.instance.player.stat.Get(StatType.MAX_HP), (int)GameManager.instance.player.stat.Get(StatType.DAMAGE),
				(int)GameManager.instance.player.stat.Get(StatType.ATTACK_SPEED), (int)GameManager.instance.player.stat.Get(StatType.ATTACK_DISTANCE),
				(int)GameManager.instance.player.stat.Get(StatType.MOVE_SPEED), (int)GameManager.instance.player.stat.Get(StatType.EXP_RANGE));
		}
		else
		{
			statImg.SetActive(false);
		}
	}



	public void QuitGame()
	{
		Application.Quit(); // 게임 종료
	}

	[ContextMenu("")]
	public void AddRune()
	{
		GameObject tempImg = Instantiate(augImg);
		tempImg.transform.SetParent(augScroll.content);
	}
	

	public void SetAugmentation()
	{
		List<string> tempAugList = new List<string>();
		var augKeys = new List<string>(GameManager.instance.augTable[0].Keys);
		for (int i = 0; i < augButtons.Length; i++)
		{
			int errorCheck = 0;     //prevention error

			int rand = -1;
			while (errorCheck < 100)
			{
				rand = UnityEngine.Random.Range(0, GameManager.instance.augTable[0].Count);
				if (tempAugList.Count > 0)
				{
					if (tempAugList.Exists((n) => n == augKeys[rand]))
					{
						errorCheck++;
					}
					else
					{
						if (GameManager.instance.player.HasAugmentation(augKeys[rand]) == false)
						{
							tempAugList.Add(augKeys[rand]);
							break;
						}
						else if (GameManager.instance.player.HasAugmentation(augKeys[rand]) &&
							GameManager.instance.player.GetAugmentationLevel(augKeys[rand]) < GameManager.instance.GetAugMaxLevel(augKeys[rand]))
						{
							tempAugList.Add(augKeys[rand]);
							break;
						}

						else
						{
							errorCheck++;
						}
					}
				}
				else
				{

					if (GameManager.instance.player.HasAugmentation(augKeys[rand]) == false)
					{
						tempAugList.Add(augKeys[rand]);
						break;
					}

					else if (GameManager.instance.player.HasAugmentation(augKeys[rand]) == true &&
							GameManager.instance.player.GetAugmentationLevel(augKeys[rand]) < GameManager.instance.GetAugMaxLevel(augKeys[rand]))
					{
						tempAugList.Add(augKeys[rand]);
						break;
					}
					else
					{
						errorCheck++;
					}
				}
			}
			if (errorCheck >= 100)
				tempAugList.Add("TempAug");

		}


		for (int i = 0; i < augButtons.Length; i++)
		{
			augButtons[i].onClick.RemoveAllListeners();
			augNameTexts[i].text = tempAugList[i];
			Augmentation aug = Activator.CreateInstance(Type.GetType(tempAugList[i]), 1, GameManager.instance.GetAugMaxLevel(tempAugList[i])) as Augmentation;
			augButtons[i].onClick.AddListener(() => {
				GameManager.instance.player.AddAugmentation(aug);
				augPanel.SetActive(false);
				Time.timeScale = 1;
			});
		}
	}
}
