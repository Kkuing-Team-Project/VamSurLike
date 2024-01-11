using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace VamSureLikeSystem.MenuSystem
{
    public class MainUIManager : MonoBehaviour
    {
        public void QuitGame()
        {
            Application.Quit(); // 게임 종료
        }
    }
}