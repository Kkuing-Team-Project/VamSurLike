using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneCtrl : MonoBehaviour
{
    public Image fadeImage;
    
    public void SelectStage(string stage)
    {
        GameManager.instance.stageName = stage;
    }

    public void TransitionToScene(string scene)
    {
        GameManager.instance.LoadInGame(scene);
    }

    public void SelectCharacter(string character)
    {
        GameManager.instance.playerName = character;
        StartCoroutine(SelectCharacterCor(1));
    }

    private IEnumerator SelectCharacterCor(float fadeTime)
    {
        SoundManager.Instance.PlayOneShot("Sound_UI_UP_Select");
        fadeImage.gameObject.SetActive(true);
        for (float elapsedTime = 0; elapsedTime < fadeTime; elapsedTime += Time.deltaTime) 
        {
            fadeImage.color = Color.Lerp(Color.clear, Color.black, elapsedTime / 0.5f);
            yield return null;
        }
        GameManager.instance.LoadInGame("InGameScene");
    }
}
