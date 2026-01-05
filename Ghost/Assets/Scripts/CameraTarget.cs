using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CameraTarget : MonoBehaviour
{
    
    GameObject item;
    itemMove script;
    public GameObject player;
    public float rotationSpeed = 1;
    ProBuilderMesh[] target;
    int index = 0;
    
    private void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("[CameraTarget] Player reference not assigned!");
        }
    }
    
    void Update()
    {
        script = FindObjectOfType<itemMove>(); // finds script - Consider caching this in the future
        if (script != null)
        {
            item = script.gameObject; //finds objects that have script
            transform.position = item.transform.position; // sets position to item that is possessed
        }
        else
        {
            if (player != null)
            {
                transform.position = player.transform.position;
            }
            else
            {
                Debug.LogWarning("[CameraTarget] Player reference is null and no possessed item found!");
            }
        }
    }
}
    

