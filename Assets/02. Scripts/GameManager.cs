using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject SoundManagerObj;
    public GameObject loadingPanel;
    public Image fadeImage;
    public Text loadingPercentage;
    [HideInInspector]
    public PlayableCtrl player;
    public GameObject[] Characters;
    public List<Dictionary<string, object>> statTable;
    public List<Dictionary<string, object>> levelTable;
    public List<Dictionary<string, object>> augTable;
    public List<Dictionary<string, object>> explanationTable;




    public string stageName;
    public string playerName;

    public bool isFinishGame;

    private CinemachineVirtualCamera followCam;
    private Coroutine fadeCor;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);


        SceneManager.sceneLoaded += OnSceneLoaded;
        statTable = CSVReader.Read("Data/Character_Enemy_Boss_Stat_Chart");
        levelTable = CSVReader.Read("Data/Character_Level_Chart");
        augTable = CSVReader.Read("Data/Reinforce_Chart");
        explanationTable = CSVReader.Read("Data/Aug_Explanation_Chart");
        loadingPanel.SetActive(false);
        // SoundManagerObj.SetActive(true); 


    }

    private void Start()
    {
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SoundManager.Instance.playerTransform = Camera.main.transform;
        switch(scene.name)
        {
            case "Main":
                SoundManager.Instance.PlaySound("Sound_BG_Title", false, default, true);
                break;
            case "Stage":
                SoundManager.Instance.PlaySound("Sound_BG_Stage_Choice", false, default, true);
                break;
            case "InGameScene":
                SoundManager.Instance.PlayOneShot("Sound_EF_CH_Spawn");
                SoundManager.Instance.PlaySound("Sound_BG_Battle01", false, default, true);

                GameObject playerObj = null;
                if (!string.IsNullOrEmpty(playerName))
                {
                    switch (playerName)
                    {
                        case "P_FIRE":
                            playerObj = Instantiate(Characters[0], Vector3.zero, Quaternion.identity);
                            break;
                        case "P_WIND":
                            playerObj = Instantiate(Characters[1], Vector3.zero, Quaternion.identity);
                            break;
                        case "P_ICE":
                            playerObj = Instantiate(Characters[2], Vector3.zero, Quaternion.identity);
                            break;
                        default:
                            Debug.Log("알 수 없는 캐릭터 이름: " + playerName);
                            break;
                    }
                    if (playerObj != null)
                    {
                        Debug.Log(playerName + " 생성 완료");
                        // Cinemachine 카메라의 Follow와 LookAt 설정
                        followCam = FindObjectOfType<CinemachineVirtualCamera>();
                        if (followCam != null)
                        {
                            followCam.Follow = playerObj.transform;
                            followCam.LookAt = playerObj.transform;
                        }
                    }
                }
                else
                {
                    Debug.Log("캐릭터 정보가 없습니다.");
                }
                break;
        }
        player = FindObjectOfType<PlayableCtrl>();
    }



    public IEnumerator LoadAsyncScene(string sceneName)
    {
        loadingPanel.SetActive(true); 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        CoroutineHandler.StopAllCoroutines();
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
            loadingPercentage.text = $"Loading...({Mathf.Round(asyncLoad.progress * 100f).ToString()}%)";
        }
        loadingPanel.SetActive(false); 
    }

    public void LoadInGame(string sceneName)
    {
        StartCoroutine(LoadAsyncScene(sceneName));
    }

    public int GetAugMaxLevel(string key)
    {
        int result = 0;
        for (int i = 0; i < augTable.Count; i++)
        {
            if (augTable[i][key].ToString() == "NONE")
                break;
            result++;
        }
        return result;
    }

    public void Fade(Color start, Color end, float time)
    {
        if(fadeCor != null)
        {
            StopCoroutine(fadeCor);
            fadeCor = null;
        }
        StartCoroutine(instance.FadeCor(start, end, time));
    }

    public IEnumerator FadeCor(Color start, Color end, float time)
    {
        fadeImage.gameObject.SetActive(true);
        for (float elapsedTime = 0; elapsedTime < time; elapsedTime += Time.deltaTime)
        {
            fadeImage.color = Color.Lerp(start, end, elapsedTime / time);
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }
}
