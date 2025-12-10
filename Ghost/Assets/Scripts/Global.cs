using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Global : MonoBehaviour
{
    //This makes script public
    public static Global Instance { get; private set; }

    
    private void Awake()
    {
        

    }

    void Update()
    {
       

    }

    public void StartGame()
    {
        SceneManager.LoadScene("LOBBY");
    }



}

