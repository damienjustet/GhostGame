using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowValue : MonoBehaviour
{
    Transform mainCam;
    Transform canvas;
    public GameObject theirParent;
    posseion ps;
    GameObject player;
    Text thisText;

    float maxOpacity = 0.85f;

    float easeTime = 0.1f;
    float ease = 0;
    public int easeDirection = -1;
    float easeTimer;

    void Awake()
    {
        ps = gameObject.GetComponentInParent<posseion>();
        player = GameObject.Find("player(Clone)");
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
        
        if (easeTimer <= Mathf.PI / 2 && easeDirection == 1 && !LevelLogic.Instance.isPossessed)
        {
            easeTimer += Time.deltaTime / (easeTime / Mathf.PI * 2);
        }
        else if (easeTimer >= 0 && (easeDirection == -1 || LevelLogic.Instance.isPossessed))
        {
            easeTimer -= Time.deltaTime / (easeTime / Mathf.PI * 2);
        }
        ease = maxOpacity * Mathf.Sin(easeTimer);
        thisText.color = new Color(thisText.color.r,thisText.color.g,thisText.color.b, ease);
    }
}
