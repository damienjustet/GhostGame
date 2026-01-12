using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public bool end = false;

    public void GameOver()
    {
        end = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
