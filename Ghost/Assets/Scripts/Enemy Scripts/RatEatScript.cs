using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RatEatScript : MonoBehaviour
{
    public int timer = 100;
    int attacks = 0;
    
    void OnTriggerStay(Collider item)
    {
        if (item.gameObject.CompareTag("Collectable"))
        {
            ItemCost itemCost = item.GetComponent<ItemCost>();
            if (itemCost == null)
            {
                Debug.LogError($"[RatEatScript] ItemCost component missing on {item.gameObject.name}!");
                return;
            }
            
            if (timer <= 0)
            {
                attacks += 1;
                itemCost.value -= 5;
                
                if (itemCost.shownText != null)
                {
                    itemCost.shownText.text = "-5";
                }
                
                if (itemCost.loseValueText != null)
                {
                    Instantiate(itemCost.loseValueText, item.transform);
                }
                else
                {
                    Debug.LogWarning($"[RatEatScript] loseValueText prefab is null on {item.gameObject.name}");
                }
                
                timer = 100;
                
                if (attacks >= 4)
                {
                    RatTargetScript ratScript = gameObject.GetComponentInParent<RatTargetScript>();
                    if (ratScript != null)
                    {
                        ratScript.scurryAway = true;
                        Debug.Log($"[RatEatScript] Rat {gameObject.name} scurrying away after 4 attacks.");
                    }
                    else
                    {
                        Debug.LogError("[RatEatScript] RatTargetScript not found in parent!");
                    }
                }
            }
            else
            {
                timer -= 1;
            }
        }
    }
}
