using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public KeyPressVisualizer keyVisual;

    public float canvasWidth;
    public float canvasHeight;

    public GameOverAnimation gameOverAnimation;

    string[] gameplayScenes = {"LEVEL1", "TUTORIAL", "House"};
    string[] blankScenes = {"TITLESCREEN"};

    public GameObject playerObj;
    public Player playerScript;
    public CapsuleCollider playerCollider;

    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (gameplayScenes.Contains(SceneManager.GetActiveScene().name))
        {
            gameplay = true;
        }
        else
        {
            gameplay = false;
        }

        playerObj = GameObject.Find("player(Clone)");
        playerScript = playerObj.GetComponent<Player>();
        playerCollider = playerObj.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))// Locks Cursor for rotating
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else // Unlocks Cursor
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) //For Exiting Play to free Mouse
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void StartGame()
    {
        LoadAScene("LOBBY");
        gameplay = false;
    }

    
    public void LoadAScene(string sceneName)
    {
        StartCoroutine(LoadATheScene(sceneName));
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    IEnumerator LoadATheScene(string sceneName)
    {

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
        
        if (gameplayScenes.Contains(sceneName))
        {
            gameplay = true;
        }
        else
        {
            gameplay = false;
        }
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

    public void InteractKeyChange(string key)
    {
        if (key == "E")
        {
            keyVisual.ShowKey("E");
        }
        else
        {
            keyVisual.ShowKey("");
        }
    }

    
}

