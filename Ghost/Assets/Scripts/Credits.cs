using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Credits : MonoBehaviour

{
    [SerializeField] private string newGameTItlescreen = "TItle screen";

    public void newGameButton()
    {
        SceneManager.LoadScene(newGameTItlescreen);

    }
}