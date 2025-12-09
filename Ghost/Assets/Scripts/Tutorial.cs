using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject textStuff;
    public Text text1;
    bool tutorialStarted = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "player(Clone)")
        {
            textStuff.SetActive(true);
            tutorialStarted = true;
        }
    }
    void Update()
    {
        if (tutorialStarted && Global.Instance.isPossessed)
        {
            text1.text = "As a possessed object you can move around and go up and down with space and shift. If you hold control and right click you can move the mouse to rotate the object. Press E to depossess the object.";
        }
    }
}
