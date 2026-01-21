using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUpAndDownTrigger : MonoBehaviour
{
    public int cutsceneIndex;
    bool canPlayCutscene = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "player(Clone)")
        {
            canPlayCutscene = true;
        }
        
    }

    void Update()
    {
        if (canPlayCutscene && LevelLogic.Instance.isPossessed)
        {
            CutsceneManager.Instance.StartCutscene(cutsceneIndex);
            Destroy(this);
        }
    }
}
