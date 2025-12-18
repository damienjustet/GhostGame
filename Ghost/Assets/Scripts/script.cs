using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class script : MonoBehaviour
{
    [SerializeField] private string newGameSettings = "Settings";
     
    public void NewGameButton()
    {
        SceneManager.LoadScene(newGameSettings);
    }

}
