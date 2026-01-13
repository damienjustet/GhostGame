using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCamera : MonoBehaviour
{
    public Transform mainCam;
    void Start()
    {
        transform.position = mainCam.position;
        transform.rotation = mainCam.rotation;
        GetComponent<Camera>().targetTexture = Camera.main.GetComponent<Camera>().targetTexture;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mainCam.position;
        transform.rotation = mainCam.rotation;
    }
}
