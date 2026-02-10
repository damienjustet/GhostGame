using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorHinge : MonoBehaviour
{
    [Range(-1,1)] public int direction;
    bool isOpen = false;
    [HideInInspector] public bool inArea = false;

    AudioSource doorSoundSource;
    
    void Awake()
    {
        doorSoundSource = gameObject.AddComponent<AudioSource>();
        SoundManager.environmentSources.Add(doorSoundSource);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && inArea)
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

    public void DoorInteract()
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

    public void OpenDoor()
    {
        isOpen = true;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += 90 * direction;
        transform.rotation = Quaternion.Euler(rotation);
        SoundManager.PlaySoundWithSource(doorSoundSource, SoundType.DOOROPEN);
    }

    void CloseDoor()
    {
        isOpen = false;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y -= 90 * direction;
        transform.rotation = Quaternion.Euler(rotation);
        SoundManager.PlaySoundWithSource(doorSoundSource, SoundType.DOORCLOSE);
    }

    public void MouseOver()
    {
        
    }

    
}
