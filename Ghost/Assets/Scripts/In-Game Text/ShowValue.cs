using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowValue : MonoBehaviour
{
    Transform mainCam;
    Transform canvas;
    public GameObject theirParent;

    void Awake()
    {
        if (GameObject.Find("Main Camera(Clone)") != null)
        {
            mainCam = GameObject.Find("Main Camera(Clone)").transform;
        }
        canvas = GameObject.Find("Floating Text Canvas").transform;

        transform.SetParent(canvas);
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCam != null)
        {
            mainCam = GameObject.Find("Main Camera(Clone)").transform;
            transform.rotation = mainCam.transform.rotation;
        }
        else if (GameObject.Find("Main Camera(Clone)") != null)
        {
            mainCam = GameObject.Find("Main Camera(Clone)").transform;
        }
        if (theirParent == null)
        {
            Destroy(gameObject);
        }
    }
}
