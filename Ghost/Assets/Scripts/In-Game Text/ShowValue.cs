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
        mainCam = GameObject.Find("Main Camera").transform;
        canvas = GameObject.Find("Floating Text Canvas").transform;

        transform.SetParent(canvas);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        if (theirParent == null)
        {
            Destroy(gameObject);
        }
    }
}
