using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SceneStart : MonoBehaviour
{
    public CinemachineVirtualCamera npcCamera;
    public CinemachineVirtualCamera playerCamera;
    public float cutsceneDuration = 5f;

    private void Start()
    {
        StartCoroutine(PlayCutscene());

    }
    IEnumerator PlayCutscene()
    {
        //Switch to NPC camera 
        npcCamera.Priority = 20;
        playerCamera.Priority = 10;

        //Wait while camera pans
        yield return new WaitForSeconds(cutsceneDuration);

        //Switch back to player camera 
        playerCamera.Priority = 20;
        npcCamera.Priority = 10;
    }
}


   