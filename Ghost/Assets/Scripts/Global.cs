using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    //This makes script public
    public static Global Instance { get; private set; }
    public bool playerLiving = true;

    SoundManager sm;

    private void Awake()
    {
        sm = GameObject.Find("SoundEffects(Clone)").GetComponent<SoundManager>();
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
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

    public void StartGame()
    {
        Debug.Log("YEP");
        SceneManager.LoadScene("LOBBY");
    }

    // checks if ghost is possessed
    public bool isPossessed = false;

    //checks if the player can interact with object(This is for the player script)
    public bool interact;

    // MONEY
    public float money;
}

