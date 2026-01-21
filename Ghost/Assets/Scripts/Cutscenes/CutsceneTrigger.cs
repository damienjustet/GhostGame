using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public int cutsceneIndex;
    public bool useTrigger;

    [HideInInspector] public bool playCutscene;
    public MonoBehaviour script;

    public BoolInspector ha;


    bool cutscenePlayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (useTrigger && !cutscenePlayed && playCutscene)
        {
            if (other.gameObject.name == "player(Clone)")
            {
                CutsceneManager.Instance.StartCutscene(cutsceneIndex);
                cutscenePlayed = true;
            
                Destroy(GetComponent<CutsceneTrigger>());
            }
        }
    }

    void Update()
    {
        if (!useTrigger && !cutscenePlayed && playCutscene)
        {
            CutsceneManager.Instance.StartCutscene(cutsceneIndex);
            cutscenePlayed = true;

            Destroy(GetComponent<CutsceneTrigger>());
        }
    }
}