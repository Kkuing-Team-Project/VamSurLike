using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class HUD : MonoBehaviour
{
	public Text timeText;
	public Text killCountText;
	public Text levelText;
	public Slider expSlider;
	public GameObject augImg;
	public ScrollRect augScroll;


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
}
