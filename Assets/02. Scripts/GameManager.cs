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
        statTable = CSVReader.Read("Data/Character_Enemy_Stat_Chart");
        levelTable = CSVReader.Read("Data/CharacterLevelChart");
        loadingPanel.SetActive(false);
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

    public void LoadInGame()
    {
        StartCoroutine(LoadAsyncScene("InGameScene"));
    }
}
