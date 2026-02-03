using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RatDespawner : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.name == "Rat(Clone)" && GetComponent<EnemyManager>().RatSpawn == false){

            Destroy(other.gameObject);
            
           
        }
    }
}
