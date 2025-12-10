using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonCredits : MonoBehaviour
{
    [SerializeField] private string newGameCredits = "Credits";

    public void NewGameButton()
    {
        SceneManager.LoadScene(newGameCredits);

    }
}
