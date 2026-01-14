using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopeAnimAttack : MonoBehaviour
{
    public void AnimFinished()
    {
        Global.Instance.gameOverAnimation.ComeHere();
        GetComponent<AudioSource>().Play();
        Camera.main.transform.Find("cameraLockTarget").GetComponent<CameraTarget>().player = gameObject.transform.parent.gameObject;
        LevelLogic.Instance.playerLiving = false;
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.6f);
    }
    
}
