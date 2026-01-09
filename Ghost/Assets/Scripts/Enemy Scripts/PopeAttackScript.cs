using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopeAttackScript : MonoBehaviour
{
    private itemMove cachedItemMove; // Cache to avoid repeated FindObjectOfType
    private bool wasPossessed = false; // Track possession state changes
    
    void Update()
    {
        // Clear cache if possession state changed
        if (LevelLogic.Instance != null && wasPossessed != LevelLogic.Instance.isPossessed)
        {
            cachedItemMove = null;
            wasPossessed = LevelLogic.Instance.isPossessed;
        }
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if (LevelLogic.Instance == null)
        {
            Debug.LogError("[PopeAttackScript] LevelLogic.Instance is null!");
            return;
        }
        
        if (!LevelLogic.Instance.isPossessed)
        {
            if (collider.gameObject.name == "player(Clone)")
            {
                Animator anim = GameObject.Find("POPE 1").GetComponent<Animator>();
                collider.gameObject.GetComponent<Player>().moveSpeed = 0;
                collider.gameObject.transform.position = transform.position;
                anim.SetTrigger("Clap");
                
                
                
                
            }
        }
        else
        {
            // Update cache if needed
            if (cachedItemMove == null)
            {
                cachedItemMove = FindObjectOfType<itemMove>();
            }
            
            if (cachedItemMove != null && collider.gameObject == cachedItemMove.gameObject)
            {
                 Animator anim = GameObject.Find("POPE 1").GetComponent<Animator>();
                collider.gameObject.GetComponent<itemMove>().moveSpeed = 0;
                collider.gameObject.transform.position = transform.position;
                anim.SetTrigger("Clap");
                
                
            }
        }
    }

}
