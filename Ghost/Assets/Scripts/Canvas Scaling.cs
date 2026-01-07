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
        GameObject mainCamera = GameObject.Find("Main Camera(Clone)");
        if (mainCamera != null)
        {
            PixelArtCamera pixelCam = mainCamera.GetComponent<PixelArtCamera>();
            if (pixelCam != null)
            {
                pixelCam.UpdateRenderTexture();
            }
        }
        
        GameObject levelLogic = GameObject.Find("Level Logic(Clone)");
        if (levelLogic != null)
        {
            LevelLogic logic = levelLogic.GetComponent<LevelLogic>();
            if (logic != null)
            {
                logic.UpdateTextPos();
            }
        }
        
        rt = GetComponent<RectTransform>();
        thisHeight = rt.localScale.x * rt.rect.height;
        thisWidth = rt.localScale.x * rt.rect.width;

        
        rt.sizeDelta = new Vector2(Screen.width, Screen.height);
        
        rt.anchoredPosition = new Vector2(Screen.width, Screen.height) / 2;

        GameObject keyPress = GameObject.Find("KeyPressVisualizer");
        RectTransform kpr = keyPress.GetComponent<RectTransform>();
        kpr.anchoredPosition = new Vector2(0, -Screen.height / 3);
        kpr.localScale = new Vector3(1,1,1);
    }

}
