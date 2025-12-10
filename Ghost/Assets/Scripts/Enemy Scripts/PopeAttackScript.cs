using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopeAttackScript : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "player(Clone)")
        {
            LevelLogic.Instance.health -= 100;
            
        }
    }
}
