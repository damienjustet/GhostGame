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
                Debug.Log("[PopeAttackScript] Pope caught the player! Returning to LOBBY.");
                SceneManager.LoadScene("LOBBY");
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
                Debug.Log("[PopeAttackScript] Pope caught the possessed item! Returning to LOBBY.");
                SceneManager.LoadScene("LOBBY");
            }
        }
    }
}
