using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RatEatScript : MonoBehaviour
{
    public int timer = 100;
    int attacks = 0;
    void OnTriggerStay(Collider item){
       
        if (item.gameObject.CompareTag("Collectable")){
            if (timer <= 0)
            {
                attacks += 1;
                item.GetComponent<ItemCost>().value -= 5;
                item.GetComponent<ItemCost>().shownText.text = "-5";
                Instantiate(item.GetComponent<ItemCost>().loseValueText, item.transform);
                timer = 100;
                if (attacks >= 4)
                {
                    gameObject.GetComponentInParent<RatTargetScript>().scurryAway = true;
                }
            }
            else
            {
                timer -= 1;
            }
            
        }
        
    }
}
