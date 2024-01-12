using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

		levelText.text = $"Lv. {(GameManager.instance.player.level + 1).ToString()}";

		if (GameManager.instance.player.level >= GameManager.instance.levelTable.Count - 1)
			expSlider.value = 1;
		else
			expSlider.value = GameManager.instance.player.exp / GameManager.instance.player.requireExp;

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
		augButtons[0].onClick.RemoveAllListeners();
		augButtons[0].onClick.AddListener(() => { 
			GameManager.instance.player.AddAugmentation(new DamageUp(1, GameManager.instance.GetAugMaxLevel("C_damage up"), AugmentationEventType.ON_UPDATE));
            augPanel.SetActive(false);
            Time.timeScale = 1;
		});
	}
}
