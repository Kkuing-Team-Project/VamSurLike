using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject ui;
    public PlayableCtrl player;
    public List<Dictionary<string, object>> statTable;

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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ui.SetActive(true);
        player = FindObjectOfType<PlayableCtrl>();
        switch (scene.name)
        {
            case "Yeopseung":

                Debug.Log(player.gameObject.name);
                break;
            default:

                ui.SetActive(true);
                break;
        }
    }

    public IEnumerator LoadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void LoadInGame()
    {
        StartCoroutine(LoadAsyncScene("Yeopseung"));
    }
}
