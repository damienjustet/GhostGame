using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHinge : MonoBehaviour
{
    [Range(-1,1)] public int direction;
    bool isOpen = false;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (isOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y += 90 * direction;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y -= 90 * direction;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
