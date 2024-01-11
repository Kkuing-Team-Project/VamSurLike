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
    public HUD inGameUI;
    public Text loadingPercentage;
    public PlayableCtrl player;
    public List<Dictionary<string, object>> statTable;
    public List<Dictionary<string, object>> levelTable;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        statTable = CSVReader.Read("Data/StatTable");
        levelTable = CSVReader.Read("Data/CharacterLevelChart");
        loadingPanel.SetActive(false);
    }

    private void Start()
    {
        inGameUI.gameObject.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = FindObjectOfType<PlayableCtrl>();
        switch (scene.name)
        {
            default:
                inGameUI.gameObject.SetActive(true);
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
        StartCoroutine(LoadAsyncScene("Yeopseung"));
    }
}
