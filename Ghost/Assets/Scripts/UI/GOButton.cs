using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GOButton : MonoBehaviour
{
    Button myButton;
    public string scene;

    void Start()
    {
        myButton = GetComponent<Button>();
    }

    public void ButtonClicked()
    {
        Global.Instance.LoadAScene(scene);
    }
}
