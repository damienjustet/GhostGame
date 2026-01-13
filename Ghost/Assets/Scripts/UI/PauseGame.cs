using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    
    public void PauseTheGame()
    {
        Time.timeScale = 0;
    }

    
    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }
}
