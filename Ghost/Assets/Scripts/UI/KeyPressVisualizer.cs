using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KeyPressVisualizer : MonoBehaviour
{
    public SpriteObject[] sprites;
    public Image image;

    float easeTime = 0.1f;
    float ease = 0;
    int easeDirection;
    float easeTimer;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        Global.Instance.keyVisual = this;
    }

    public void ShowKey(string keyName)
    {
        if (keyName != "")
        {
            easeDirection = 1;
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
            easeDirection = -1;
        }
        
        
    }

    void Update()
    {
        if (easeTimer <= Mathf.PI / 2 && easeDirection == 1)
        {
            easeTimer += Time.deltaTime / (easeTime / Mathf.PI * 2);
            ease = Mathf.Sin(easeTimer);
            
            image.color = new Color(255,255,255, ease);
        }
        else if (easeTimer >= 0 && easeDirection == -1)
        {
            easeTimer -= Time.deltaTime / (easeTime / Mathf.PI * 2);
            ease = Mathf.Sin(easeTimer);
            
            image.color = new Color(255,255,255, ease);
        }
    }
}

[Serializable]
public class SpriteObject
{
    public string name = null;
    public Sprite sprite = null;
}
