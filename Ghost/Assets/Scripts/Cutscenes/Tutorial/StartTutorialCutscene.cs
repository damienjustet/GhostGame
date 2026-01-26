using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTutorialCutscene : MonoBehaviour
{
    void Start()
    {
        CutsceneManager.Instance.StartCutscene(0);
    }

}
