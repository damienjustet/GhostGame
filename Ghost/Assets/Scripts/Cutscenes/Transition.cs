using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Transition : MonoBehaviour
{
    public Animator animator;
    
    public void StartTransition()
    {
        animator.SetTrigger("Start Transition");
        Time.timeScale = 0;
    }

    public void SceneCanLoad()
    {
        print("YAR");
        Global.Instance.SceneLoadReady();
    }

    public void UnTransition()
    {
        animator.SetTrigger("Untransition");
    }
    
    public void SceneLoaded()
    {
        Time.timeScale = 1;
    }

}


