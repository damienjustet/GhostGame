using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAnimation : MonoBehaviour
{
    float timer = 0;
    float waitTime;
    public Animator leftEye;
    public Animator rightEye;

    void Start()
    {
        waitTime = Random.Range(5,10);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            leftEye.SetTrigger("Blink");
            rightEye.SetTrigger("Blink");
            timer = 0;
            waitTime = Random.Range(5,10);
        }
    }
}
