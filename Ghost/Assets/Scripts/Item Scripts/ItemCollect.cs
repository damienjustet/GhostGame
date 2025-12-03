using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollect : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Collectable")
        {
            Global.Instance.money += collider.gameObject.GetComponent<ItemCost>().value;
            
            collider.GetComponent<ItemCost>().Collect(transform.position + Vector3.right);
            
        }
    }
}
