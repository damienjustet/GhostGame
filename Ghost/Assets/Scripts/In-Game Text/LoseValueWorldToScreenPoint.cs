using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseValueWorldToScreenPoint : MonoBehaviour
{
    [HideInInspector] public Vector3 place;
    Transform canvas;
    RectTransform uiElement;
    Camera main;
    public float aliveTime;
    float timer = 0;

    Vector3 screenPos;

    Text thisText;

    // Start is called before the first frame update
    void Awake()
    {
        main = GameObject.Find("Main Camera").GetComponent<Camera>();
        uiElement = GetComponent<RectTransform>();
        screenPos = main.WorldToScreenPoint(place);
        screenPos.y = Screen.height - screenPos.y;
        canvas = GameObject.Find("Canvas").transform;
        transform.SetParent(canvas);
        thisText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // if (aliveTime <= timer)
        // {
        //     Destroy(gameObject);
        // }
        // else if (aliveTime / 2 <= timer)
        // {
        //     thisText.color -= new Color(0, 0, 0, 1) * Time.deltaTime * 2;
        // }
        //place -= new Vector3(0, 1, 0) * Time.deltaTime;
        screenPos = main.WorldToScreenPoint(place);
        timer += Time.deltaTime;
        uiElement.position = screenPos;
    }
}
