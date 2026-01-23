using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositTrigger : MonoBehaviour
{
    public int cutsceneIndex;
    float timer = 0;

    void Start()
    {
        if (!Global.Instance.firstTime)
        {
            Destroy(this);
        }
    }

    void Update()
    {
        if (LevelLogic.Instance.money > 0)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                CutsceneManager.Instance.StartCutscene(cutsceneIndex);
                Destroy(this);
            }
        }
    }
}
