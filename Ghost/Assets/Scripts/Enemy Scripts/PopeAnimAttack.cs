using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopeAnimAttack : MonoBehaviour
{
    public GameOverAnimation gameOverScreen;

    public void AnimFinished()
    {
        gameOverScreen.ComeHere();
        GetComponent<AudioSource>().Play();
        StartCoroutine(wait());
        
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.6f);
    }
}
