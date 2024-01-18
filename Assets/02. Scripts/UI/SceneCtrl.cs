using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{

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

    }
}
