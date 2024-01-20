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
        GameManager.instance.Fade(Color.clear, Color.black, 1f);
        yield return new WaitForSeconds(1f);
        GameManager.instance.LoadInGame("InGameScene");
    }
}
