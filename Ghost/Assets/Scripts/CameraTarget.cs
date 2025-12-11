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
            target = FindObjectsOfType<ProBuilderMesh>();
            
            transform.position = player.transform.position;
            
        }}
        
        


    }
    

