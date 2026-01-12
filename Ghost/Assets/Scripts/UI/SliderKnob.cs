using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderKnob : MonoBehaviour
{
    Image imageComponent;
    Color ogColor;

    bool pointerOver = false;
    RectTransform rt;

    bool dragging = false;

    
    void Start()
    {
        imageComponent = GetComponent<Image>();
        ogColor = imageComponent.color;
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (pointerOver && Input.GetMouseButton(0) || dragging)
        {
            if (dragging && Input.GetMouseButton(0))
            {
                rt.anchoredPosition = new Vector2(Input.mousePosition.x - Global.Instance.canvasWidth / 2, rt.anchoredPosition.y);
            }
            else if (dragging)
            {
                dragging = false;
            }
            else
            {
                dragging = true;
            }
            
        }
    }
    public void MouseIsOver()
    {
        imageComponent.color = Color.white;
        pointerOver = true;
    }

    public void MouseNotOver()
    {
        imageComponent.color = ogColor;
        pointerOver = false;
    }

}
