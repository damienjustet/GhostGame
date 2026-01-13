using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    bool paused = false;

    public void PauseTheGame()
    {
        if (!paused)
        {
            Time.timeScale = 0;
            paused = true;
        }
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        paused = false;
    }
}
