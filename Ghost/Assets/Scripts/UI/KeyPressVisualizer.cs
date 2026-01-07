using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KeyPressVisualizer : MonoBehaviour
{
    public SpriteObject[] sprites;
    public Image image;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        Global.Instance.keyVisual = this;
    }

    public void ShowKey(string keyName)
    {
        if (keyName != "")
        {
            image.color = new Color(255,255,255,255);
           foreach (SpriteObject sprite in sprites)
            {
                if (sprite.name == keyName)
                {
                    image.sprite = sprite.sprite;
                    break;
                }
            } 
        }
        else
        {
            image.color = new Color(255,255,255,0);
        }
        
    }
}

[Serializable]
public class SpriteObject
{
    public string name = null;
    public Sprite sprite = null;
}
