using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPart : MonoBehaviour
{
    DoorHinge parentHinge;

    void Start()
    {
        parentHinge = gameObject.GetComponentInParent<DoorHinge>();
    }
    void OnTriggerEnter(Collider other) // if in area
    {
        Debug.Log("In");
        if (other.gameObject.name == "detectionArea")
        {
            parentHinge.inArea = true;
            
        }

    }
    void OnTriggerExit(Collider other)// not in area
    {
        if (other.gameObject.name == "detectionArea")
        {
            parentHinge.inArea = false;
            Debug.Log("Out");
        }
    }
}
