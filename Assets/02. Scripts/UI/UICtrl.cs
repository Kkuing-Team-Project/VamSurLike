using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrl : MonoBehaviour
{
    public GameObject Canvas;
    private bool objectsActive = false;
    private bool isPaused = false; 

    // Update is called once per frame

    void Start(){
        Canvas.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ESC 키를 누르면 활성화 또는 비활성화 상태를 토글
            objectsActive = !objectsActive;
            Canvas.SetActive(objectsActive);

            TogglePause();
        }
    }

    void TogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1; // 게임 진행
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0; // 게임 일시 정지
            isPaused = true;
        }
    }
}
