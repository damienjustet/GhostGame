using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private string newGameleve1 = "leve1";

  public void NewGameButton()
    {
        SceneManager.LoadScene(newGameleve1);
    }

}
