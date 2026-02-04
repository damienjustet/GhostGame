using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Transition : MonoBehaviour
{
    public Animator animator;
    
    public void StartTransition()
    {
        animator.SetTrigger("Start Transition");
    }

    public void SceneCanLoad()
    {
        Global.Instance.SceneLoadReady();
    }

}


