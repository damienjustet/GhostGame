using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseValue : MonoBehaviour
{
    Transform mainCam;
    Transform canvas;
    public GameObject theirParent;
    public float aliveTime;
    float timer = 0;

    Text thisText;

    void Awake()
    {
        if (GameObject.Find("Main Camera(Clone)") != null)
        {
            mainCam = GameObject.Find("Main Camera(Clone)").transform;
        }
        canvas = GameObject.Find("Floating Text Canvas").transform;

        transform.SetParent(canvas);
        thisText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Main Camera(Clone)") != null && mainCam != null)
        {
            mainCam = GameObject.Find("Main Camera(Clone)").transform;
            transform.rotation = mainCam.transform.rotation;
        }
        if (aliveTime <= timer)
        {
            Destroy(gameObject);
        }
        else if (aliveTime / 2 <= timer)
        {
            thisText.color -= new Color(0, 0, 0, 1) * Time.deltaTime * 2;
        }
        transform.position += new Vector3(0, 1, 0) * Time.deltaTime;
        timer += Time.deltaTime;
    }
}
