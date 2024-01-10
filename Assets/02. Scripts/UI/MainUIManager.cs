using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace WargameSystem.MenuSystem
{
    public class MainUIManager : MonoBehaviour
    {
        public void QuitGame()
        {
            Application.Quit(); // 게임 종료
        }
    }
}