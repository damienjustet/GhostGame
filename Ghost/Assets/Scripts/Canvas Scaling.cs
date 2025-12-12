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
    public void ResizeScreen()
    {
        if (GameObject.Find("Main Camera(Clone)"))
        {
            GameObject.Find("Main Camera(Clone)").GetComponent<PixelArtCamera>().UpdateRenderTexture();
        }
        if (GameObject.Find("Level Logic(Clone)"))
        {
            GameObject.Find("Level Logic(Clone)").GetComponent<LevelLogic>().UpdateTextPos();
        }
        rt = GetComponent<RectTransform>();
        thisHeight = rt.localScale.x * rt.rect.height;
        thisWidth = rt.localScale.x * rt.rect.width;

        
        rt.sizeDelta = new Vector2(Screen.width, Screen.height);
        
        rt.anchoredPosition = new Vector2(Screen.width, Screen.height) / 2;
    }

}
