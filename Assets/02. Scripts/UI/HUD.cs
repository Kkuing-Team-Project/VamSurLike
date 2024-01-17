using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using static Cinemachine.DocumentationSortingAttribute;


public class HUD : MonoBehaviour
{
	public Text timeText;
	public Text killCountText;
	public Text levelText;
	public Slider expSlider;
	public GameObject augIconPrefab;
	public GameObject statImg;
	public Text statText;
	public GameObject iconPanel;

	public GameObject augPanel;
	public Button[] augButtons;
	public Text[] augNameTexts;
	public Text[] augExplanationTexts;
	public Text[] augTypeTexts;

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

		if(Input.GetKeyDown(KeyCode.Tab))
		{
			iconPanel.SetActive(!iconPanel.activeSelf);
		}
	}



	public void QuitGame()
	{
		Application.Quit(); // 게임 종료
	}

	public void AddRune(Augmentation aug)
	{


		if(iconPanel.transform.Find(aug.ToString()) != null)
		{
            iconPanel.transform.Find(aug.ToString()).GetComponentInChildren<Text>().text = (GameManager.instance.player.GetAugmentation(aug.ToString()).level + 1).ToString();
        }
		else
		{

            if (iconPanel.transform.childCount >= 12)
			{
				Destroy(iconPanel.transform.GetChild(iconPanel.transform.childCount - 1).gameObject);
			}
            GameObject icon = Instantiate(augIconPrefab);
            icon.transform.SetParent(iconPanel.transform);
			icon.transform.SetAsFirstSibling();
			icon.name = aug.ToString();
			icon.GetComponent<Image>().sprite = aug.icon;
			icon.transform.GetComponentInChildren<Text>().text = "1";
		}
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
							GameManager.instance.player.GetAugmentationLevel(augKeys[rand]) < GameManager.instance.GetAugMaxLevel(augKeys[rand]) - 1)
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
							GameManager.instance.player.GetAugmentationLevel(augKeys[rand]) < GameManager.instance.GetAugMaxLevel(augKeys[rand]) - 1)
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
			string key = tempAugList[i];
            augNameTexts[i].text = key;

			int level = GameManager.instance.player.HasAugmentation(key) ? GameManager.instance.player.GetAugmentationLevel(key) + 1 : 0;

			string[] datas = GameManager.instance.augTable[level][key].ToString().Split("-");

            for (int j = 0; j < GameManager.instance.augTable[0].Count; j++)
            {
                if (key.Equals(GameManager.instance.explanationTable[j]["Item_Name"].ToString()))
                {
					augNameTexts[i].text = GameManager.instance.explanationTable[j]["Korea_Name"].ToString();
					augTypeTexts[i].text = GameManager.instance.explanationTable[j]["Item_Type"].ToString();
                    string explanation = GameManager.instance.explanationTable[j]["Item_Explanation"].ToString();
                    augExplanationTexts[i].text = string.Format(explanation, datas);
                }
            }

            augButtons[i].onClick.AddListener(() =>
			{
                Augmentation aug = Activator.CreateInstance(Type.GetType(key), 0, GameManager.instance.GetAugMaxLevel(key)) as Augmentation;
                GameManager.instance.player.AddAugmentation(aug);
				AddRune(aug);
				augPanel.SetActive(false);
                Time.timeScale = 1;
			});
		}
	}

	private string GetAugExplanation(string key, params string[] datas)
	{

		return null;
	}
}
