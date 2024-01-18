using System;
using System.Collections;
using System.Collections.Generic;
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
    public int killCount;

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
