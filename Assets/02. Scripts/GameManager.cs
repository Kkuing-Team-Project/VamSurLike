using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject SoundManagerObj;
    public GameObject loadingPanel;
    public Text loadingPercentage;
    [HideInInspector]
    public PlayableCtrl player;
    public GameObject[] Characters;
    public List<Dictionary<string, object>> statTable;
    public List<Dictionary<string, object>> levelTable;
    public List<Dictionary<string, object>> augTable;
    public List<Dictionary<string, object>> explanationTable;

    private int m_killCount;
    public Animator killCountAnimator { get; set; }

    public string stageName;
    public string playerName;

    private CinemachineVirtualCamera followCam;
    
    public int killCount
    {
        get => m_killCount;
        set
        {
            m_killCount = value;
            killCountAnimator?.SetTrigger("Kill");
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        statTable = CSVReader.Read("Data/Character_Enemy_Boss_Stat_Chart");
        levelTable = CSVReader.Read("Data/Character_Level_Chart");
        augTable = CSVReader.Read("Data/Reinforce_Chart");
        explanationTable = CSVReader.Read("Data/Aug_Explanation_Chart");
        loadingPanel.SetActive(false);
        SoundManagerObj.SetActive(true); 

        player = FindObjectOfType<PlayableCtrl>();
    }

    private void Start()
    {
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "InGameScene"){
            GameObject playerObj = null;
            if(!string.IsNullOrEmpty(playerName)){
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
        }

        switch(scene.name)
        {
            case "Main":
                Debug.Log("메인씬 입니다");
                SoundManager.Instance.PlaySound("Sound_BG_Title");
                break;
            case "Stage":
                SoundManager.Instance.StopBackgroundMusic();
                SoundManager.Instance.PlaySound("Sound_BG_Stage_Choice");
                break;
            case "InGameScene":
                SoundManager.Instance.StopBackgroundMusic();
                SoundManager.Instance.PlaySound("Sound_EF_CH_Spawn");
                SoundManager.Instance.PlaySound("Sound_BG_Battle01");
                // SoundManager.Instance.PlaySound("Sound_BG_Battle02");
                break;
        }

        killCount = 0;
        player = FindObjectOfType<PlayableCtrl>();
    }



    public IEnumerator LoadAsyncScene(string sceneName)
    {
        loadingPanel.SetActive(true); 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
            loadingPercentage.text = $"Loading...({Mathf.Round(asyncLoad.progress * 100f).ToString()}%)";
        }
        yield return new WaitForSeconds(0.1f);
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
}
