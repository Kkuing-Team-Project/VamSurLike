using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HUD : MonoBehaviour
{
	public Text timeText;
	public Text timerNameText;
	public Text killCountText;
	public Text levelText;
	public Slider expSlider;
	public GameObject augIconPrefab;
	public GameObject iconPanel;
	public Image occupyPercentImage;
	public Text occupyPercentText;
	public Image skillImage;
	public Image skillCoolTimeImage;

	[Header("증강 관련")]
	public GameObject augPanel;
	public Button[] augButtons;
	public Text[] augNameTexts;
	public Text[] augExplanationTexts;
	public Text[] augTypeTexts;
	public Image[] augImage;

	[Header("Pause UI")]
	public GameObject pauseAugPanel;
	public GameObject pausePanel;
	public Text playerStatText;
	public Text staffStatText;

	public PlayerBar playerGaugeBar;

	private Spirit sprite;

	private void Start()
	{
		GameManager.instance.killCountAnimator = killCountText.GetComponentInParent<Animator>();
		sprite = FindObjectOfType<Spirit>();
		pausePanel.SetActive(false);
		augPanel.SetActive(false);
	}

	void LateUpdate()
	{
		UIUpdate();
	}

	private void UIUpdate()
	{
		if (GameManager.instance.player == null)
			return;

		// int sceneStartTime = (int)Time.timeSinceLevelLoad;
		// timeText.text = string.Format("{0:D2} : {1:D2}", sceneStartTime / 60, sceneStartTime % 60);

		killCountText.text = GameManager.instance.killCount.ToString();
		
		if (sprite.collapseZone && sprite.collapseZone.gameObject.activeSelf)
		{
			if (sprite.spiritState == SpiritState.OCCUPY)
			{
				int remainingTime = (int)(sprite.collapseZone.collapseTime - sprite.collapseZone.elapsedTime);
				timeText.color = remainingTime <= 10 ? Color.red : Color.white;
				timeText.text = string.Format("[ {0:00} : {1:00} ]", 
					remainingTime / 60,
					remainingTime % 60);
				timerNameText.text = "안정화 제한 시간";
				timeText.transform.parent.gameObject.SetActive(true);
				
				
				occupyPercentImage.transform.parent.gameObject.SetActive(true);
				occupyPercentImage.fillAmount = sprite.collapseZone.stablity / 100f;
				occupyPercentText.text = $"{sprite.collapseZone.stablity:0}%";
			}
			else
			{
				timeText.text = null;
				timerNameText.text = "균열 생성";
			}
		}
		else
		{
			timerNameText.text = "균열 생성 시간";
			timeText.color = Color.white;
			timeText.text = string.Format("[ {0:00} : {1:00} ]", (int)sprite.collapseZoneSpawner.remainingTime / 60, (int)sprite.collapseZoneSpawner.remainingTime % 60);
			occupyPercentImage.transform.parent.gameObject.SetActive(false);
		}
		timeText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(timeText.text));
		
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

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			pausePanel.SetActive(!pausePanel.activeSelf);
			Time.timeScale = pausePanel.activeSelf ? 0 : 1;

			playerStatText.text = string.Format("{0:D2}\n{1:F1}\n{2:D2}\n{3:D2}",
				(int)GameManager.instance.player.stat.Get(StatType.MAX_HP), GameManager.instance.player.healPerSec,
				(int)GameManager.instance.player.stat.Get(StatType.MOVE_SPEED), (int)GameManager.instance.player.stat.Get(StatType.EXP_RANGE));

			staffStatText.text = string.Format("{0:D2}\n{1:D2}\n{2:D2}\n{3:D2}\n{4:D2}",
                (int)GameManager.instance.player.stat.Get(StatType.DAMAGE), (int)GameManager.instance.player.stat.Get(StatType.ATTACK_SPEED),
                GameManager.instance.player.HasAugmentation<SplashShooting>() == true ? GameManager.instance.augTable[GameManager.instance.player.GetAugmentation<SplashShooting>().level]["SplashShooting"] : 0,
                GameManager.instance.player.HasAugmentation<SplitShooting>() == true ? GameManager.instance.augTable[GameManager.instance.player.GetAugmentation<SplitShooting>().level]["SplitShooting"] : 0,
                GameManager.instance.player.HasAugmentation<KnockbackShot>() == true ? GameManager.instance.augTable[GameManager.instance.player.GetAugmentation<KnockbackShot>().level]["KnockbackShot"] : 0);
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

	public void AddRuneIcon(Augmentation aug)
	{
		if(iconPanel.transform.Find(aug.ToString()) != null)
		{
			int idx = iconPanel.transform.Find(aug.ToString()).GetSiblingIndex();
            iconPanel.transform.GetChild(idx).GetComponentInChildren<Text>().text = (GameManager.instance.player.GetAugmentation(aug.ToString()).level + 1).ToString();
			pauseAugPanel.transform.GetChild(idx).GetComponentInChildren<Text>().text = (GameManager.instance.player.GetAugmentation(aug.ToString()).level + 1).ToString();
        }
		else
		{
            if (iconPanel.transform.childCount >= 6)
			{
				Destroy(iconPanel.transform.GetChild(iconPanel.transform.childCount - 1).gameObject);
				Destroy(pauseAugPanel.transform.GetChild(pauseAugPanel.transform.childCount - 1).gameObject);
			}
            GameObject icon = Instantiate(augIconPrefab);
            icon.transform.SetParent(iconPanel.transform);
			icon.transform.SetAsFirstSibling();
			icon.name = aug.ToString();
			icon.GetComponent<Image>().sprite = aug.icon;
			icon.transform.GetComponentInChildren<Text>().text = "1";

            GameObject pauseIcon = Instantiate(augIconPrefab);
            pauseIcon.transform.SetParent(pauseAugPanel.transform);
            pauseIcon.transform.SetAsFirstSibling();
            pauseIcon.name = aug.ToString();
            pauseIcon.GetComponent<Image>().sprite = aug.icon;
			pauseIcon.transform.GetComponentInChildren<Text>().text = "1";
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
			string type = string.Empty;
            augNameTexts[i].text = key;

			int level = GameManager.instance.player.HasAugmentation(key) ? GameManager.instance.player.GetAugmentationLevel(key) + 1 : 0;

			string[] datas = GameManager.instance.augTable[level][key].ToString().Split("-");

            for (int j = 0; j < GameManager.instance.augTable[0].Count; j++)
            {
                if (key.Equals(GameManager.instance.explanationTable[j]["Item_Name"].ToString()))
                {
					Sprite img = Resources.Load<Sprite>("SkillIcon/" + key + "Icon");
					if (img != null)
						augImage[i].sprite = img;
					else
						augImage[i].sprite = Resources.Load<Sprite>("SkillIcon/WizardIcon");
					augNameTexts[i].text = GameManager.instance.explanationTable[j]["Korea_Name"].ToString();
					type = GameManager.instance.explanationTable[j]["Item_Type"].ToString();
                    augTypeTexts[i].text = type;
                    string explanation = GameManager.instance.explanationTable[j]["Item_Explanation"].ToString();
                    augExplanationTexts[i].text = string.Format(explanation, datas);
                }
            }

            augButtons[i].onClick.AddListener(() =>
			{
                Augmentation aug = Activator.CreateInstance(Type.GetType(key), 0, GameManager.instance.GetAugMaxLevel(key)) as Augmentation;
                GameManager.instance.player.AddAugmentation(aug);
				SoundManager.Instance.PlayOneShot("Sound_UI_UP_Select");
				if (type.Equals("보조"))
					AddRuneIcon(aug);
				augPanel.SetActive(false);
                Time.timeScale = 1;
			});
		}
	}

	public IEnumerator CoolTimeUICor(float time)
	{
		skillCoolTimeImage.fillAmount = 1;
		skillCoolTimeImage.color = new Color32(0, 0, 0, 128);

		for (float elapsedTime = 0; elapsedTime < time; elapsedTime += Time.deltaTime) 
		{
			skillCoolTimeImage.fillAmount = 1f - elapsedTime / time;
			yield return null;
		}
		skillCoolTimeImage.fillAmount = 1f;
		skillCoolTimeImage.color = Color.white;
        for (float elapsedTime = 0; elapsedTime < 0.25f; elapsedTime += Time.deltaTime)
		{
            skillCoolTimeImage.color = Color.Lerp(Color.white, Color.clear, elapsedTime / 0.25f);
			yield return null;
		}
		skillCoolTimeImage.color = Color.clear;
    }
}
