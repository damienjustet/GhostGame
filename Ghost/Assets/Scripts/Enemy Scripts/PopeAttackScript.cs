using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopeAttackScript : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (!LevelLogic.Instance.isPossessed)
        {
            if (collider.gameObject.name == "player(Clone)")
        {
            SceneManager.LoadScene("LOBBY");
            
        }
        }
        else if (FindObjectOfType<itemMove>() != null)
        {
            if (collider.gameObject == FindObjectOfType<itemMove>().gameObject)
            {
                SceneManager.LoadScene("LOBBY");
            }
        }
        
        
    }
}
