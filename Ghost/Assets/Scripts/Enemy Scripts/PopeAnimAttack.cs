using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopeAnimAttack : MonoBehaviour
{
    public void AnimFinished()
    {
        GetComponent<AudioSource>().Play();
        StartCoroutine(wait());
        SceneManager.LoadScene("LOBBY");
        
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.6f);
    }
}
