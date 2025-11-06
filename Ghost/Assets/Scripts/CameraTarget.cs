using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    
    GameObject item;
    itemMove script;
    public GameObject player;
    public float rotationSpeed = 1;
    private void Start()
    {
        
    }
    void Update()
    {
        script = FindObjectOfType<itemMove>(); // finds script
        if (script != null)
        {
            item = script.gameObject; //finds objects that have script
            transform.position = item.transform.position; // sets position to item that is possessed
        }
        else
        {
            transform.position = player.transform.position;
        }
        
        


    }
    
}
