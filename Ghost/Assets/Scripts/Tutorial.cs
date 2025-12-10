using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject textStuff;
    public Text text1;
    bool PossessionTutorialStarted = false;

    bool possessed;
    string nextLine;
    bool isTyping = false;
    int lineIndex = 0;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "player(Clone)" && lineIndex == 0)
        {
            textStuff.SetActive(true);
            PossessionTutorialStarted = true;
            nextLine = "You can possess objects by hovering your mouse over and pressing e to interact.";
            StartCoroutine(ShowText());
        }
    }
    IEnumerator ShowText()
    {
            isTyping = true;
            text1.text = "";
            
            for(int i = 0; i < nextLine.Length; i++)
            {
                text1.text += nextLine[i];
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(1f);
            lineIndex += 1;
            isTyping = false;
            
        
    }
    void Update()
    {
         if (PossessionTutorialStarted && LevelLogic.Instance.isPossessed && Input.GetKeyDown(KeyCode.E) && lineIndex == 1)
        {
            nextLine = "As a possessed object you can move around and go up and down with space and shift. If you hold control and right click you can move the mouse to rotate the object. Press E to depossess the object.";
            StartCoroutine(ShowText());
        }
        else if(PossessionTutorialStarted && lineIndex == 2)
        {
            nextLine = "Objects have a value. Bring the objects to a collection zone to try to meet your quota. Be careful though as they can be damaged if you drop it harshly.";
            StartCoroutine(ShowText());
            
        }
        if (lineIndex == 3)
        {
            textStuff.SetActive(false); 
            lineIndex = 0;
        }
        }
}
