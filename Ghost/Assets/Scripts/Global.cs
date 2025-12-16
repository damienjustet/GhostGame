using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Global : MonoBehaviour
{
    //This makes script public
    public static Global Instance { get; private set; }

    public AsyncOperation asyncSceneLoading;

    public bool gameplay = false;

    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
       

    }

    public void StartGame()
    {
        asyncSceneLoading = SceneManager.LoadSceneAsync("LOBBY");
        gameplay = false;
    }

    // public void LoadAScene(string sceneName)
    // {
    //     asyncSceneLoading = SceneManager.LoadSceneAsync(sceneName);
    //     if (sceneName != "LOBBY")
    //     {
    //         gameplay = true;
    //     }
    //     else
    //     {
    //         gameplay = false;
    //     }
    //     while (!asyncSceneLoading.isDone)
    //     {
    //         Debug.Log("Not yet");
    //     }
    //     GameObject.Find("Scene Builder").GetComponent<SceneBuilder>().StartScene();
    // }

    public void LoadAScene(string sceneName)
    {
        StartCoroutine(LoadATheScene(sceneName));
    }

    IEnumerator LoadATheScene(string sceneName)
    {

        if (sceneName != "LOBBY")
        {
            gameplay = true;
        }
        else
        {
            gameplay = false;
        }

        // Start loading the scene asynchronously
        asyncSceneLoading = SceneManager.LoadSceneAsync(sceneName,  LoadSceneMode.Single);

        // Wait until the asynchronous scene fully loads
        while (!asyncSceneLoading.isDone)
        {
            // Optional: Update a loading bar or display progress
            Debug.Log("Loading Progress: " + asyncSceneLoading.progress);
            yield return null; // Wait for the next frame
        }
        GameObject.Find("Scene Builder").GetComponent<SceneBuilder>().StartScene();
        
    }

    public void StartMusic()
    {
        MusicType song;
        if (Enum.TryParse(SceneManager.GetActiveScene().name, out song))
        {
            SoundManager.StartSong(song);
        }
        else
        {
            Debug.Log("No music played");
        }
    }
}

