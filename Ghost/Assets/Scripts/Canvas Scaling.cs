using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScaling : MonoBehaviour
{
    float thisWidth;
    float thisHeight;
    RectTransform rt;
    // Start is called before the first frame update
    void Start()
    {
        ResizeScreen();   
    }

    // Update is called once per frame
    void ResizeScreen()
    {
        rt = GetComponent<RectTransform>();
        thisHeight = rt.localScale.x * rt.rect.height;
        thisWidth = rt.localScale.x * rt.rect.width;

        Vector2 parentSize = GetComponentInParent<RectTransform>().sizeDelta;
        Vector3 parentPos = GetComponentInParent<RectTransform>().anchoredPosition / 2;
        
        rt.sizeDelta = parentSize;
        
        rt.anchoredPosition = parentPos;
    }
}
