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
    float maxX;
    float minX;

    public AudioType audioType;

    
    void Start()
    {
        imageComponent = GetComponent<Image>();
        ogColor = imageComponent.color;
        rt = GetComponent<RectTransform>();

        maxX = gameObject.transform.Find("SliderMax").GetComponent<RectTransform>().anchoredPosition.x;
        minX = gameObject.transform.Find("SliderMin").GetComponent<RectTransform>().anchoredPosition.x;

        if (audioType == AudioType.ENVIRONMENT)
        {
            rt.anchoredPosition = new Vector2((Global.Instance.soundVolume * (maxX - minX)) + minX, rt.anchoredPosition.y);
        }
        else
        {
            rt.anchoredPosition = new Vector2((Global.Instance.musicVolume * (maxX - minX)) + minX, rt.anchoredPosition.y);
        }
        
    }

    void Update()
    {
        if (pointerOver && Input.GetMouseButton(0) || dragging)
        {
            if (dragging && Input.GetMouseButton(0))
            {
                float mouseX = Input.mousePosition.x - Global.Instance.canvasWidth / 2;
                Debug.Log(mouseX);
                if (mouseX <= minX)
                {
                    rt.anchoredPosition = new Vector2(minX, rt.anchoredPosition.y);
                }
                else if (mouseX >= maxX)
                {
                    rt.anchoredPosition = new Vector2(maxX, rt.anchoredPosition.y);
                }
                else
                {
                    rt.anchoredPosition = new Vector2(mouseX, rt.anchoredPosition.y);
                }
                print((rt.anchoredPosition.x - minX) / (maxX - minX));

                // Changes the volume
                if (audioType == AudioType.ENVIRONMENT)
                {
                    Global.Instance.soundVolume = (rt.anchoredPosition.x - minX) / (maxX - minX);
                }
                else
                {
                    Global.Instance.musicVolume = (rt.anchoredPosition.x - minX) / (maxX - minX);
                }
                SoundManager.instance.ChangeVolume(audioType);
            }
            else if (dragging)
            {
                dragging = false;
                if (!pointerOver)
                {
                    imageComponent.color = ogColor;
                }
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
        if (!dragging)
        {
            imageComponent.color = ogColor;
        }
        pointerOver = false;
        
    }

}
