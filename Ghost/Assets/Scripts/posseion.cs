using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class posseion : MonoBehaviour
{
    private bool interactable = false;
   
    bool inArea;
   
    private void OnMouseOver()
    {
 
        if (!Global.Instance.isPossessed && inArea == true)
        {
            interactable = true;
            GetComponent<Renderer>().material.color = Color.yellow; // Shows if you can click on it. This can be changed for some other effect
            Global.Instance.interact = true; // basically same as interactable var but its so player can access it though don't delete the other one because we need individual vars for the different items
        }
        
    }
    void OnMouseExit()
    {
       
        GetComponent<Renderer>().material.color = Color.white; // Resets color from yellow

        interactable = false;
        Global.Instance.interact = false;
    }
    private void Update()
    {
        if (interactable && Input.GetKeyDown(KeyCode.E))
        {
            gameObject.AddComponent<itemMove>();
            
            if (gameObject.GetComponent<Rigidbody>() != null) //Allows you to possess rigidbodys
            {
                gameObject.GetComponent<Rigidbody>().useGravity = false; 
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            
            

        }
        else if (Input.GetKeyDown(KeyCode.E)) // possess object
        {
            Destroy(gameObject.GetComponent<itemMove>());
            if (gameObject.GetComponent<Rigidbody>() != null)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                
            }
        }

    }
    private void OnTriggerEnter(Collider other) // if in area
    {
        
        if (other.gameObject.name == "detectionArea")
        {
            inArea = true;
            
        }

    }
    private void OnTriggerExit(Collider other)// not in area
    {
        if (other.gameObject.name == "detectionArea")
        {
            inArea = false;
            
        }
    }
}


