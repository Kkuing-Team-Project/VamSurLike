using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject loadingPanel;
    public Text loadingPercentage;
    [HideInInspector]
    public PlayableCtrl player;
    public List<Dictionary<string, object>> statTable;
    public List<Dictionary<string, object>> levelTable;
    public List<Dictionary<string, object>> augTable;
    public List<Dictionary<string, object>> explanationTable;
    private int m_killCount;
    public Animator killCountAnimator { get; set; }

    public string stageName;
    public string playerName;
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

        player = FindObjectOfType<PlayableCtrl>();
    }

    private void Start()
    {
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        killCount = 0;
        if (playerName != null)
        {
          // 여기에 player type name 정의한 값
        }
        
        player = FindObjectOfType<PlayableCtrl>();
        switch (scene.name)
        {
            default:
                break;
        }
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
