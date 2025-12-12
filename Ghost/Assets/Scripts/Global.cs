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

    public void LoadAScene(string sceneName)
    {
        asyncSceneLoading = SceneManager.LoadSceneAsync(sceneName);
        if (sceneName != "LOBBY")
        {
            gameplay = true;
        }
        else
        {
            gameplay = false;
        }
    }



}

