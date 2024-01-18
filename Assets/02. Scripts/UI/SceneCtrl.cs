using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    // 새로운 장면으로 전환하는 메소드
    
    public void TransitionToScene(string sceneName)
    {
        GameManager.instance.LoadInGame(sceneName);
    }
}
