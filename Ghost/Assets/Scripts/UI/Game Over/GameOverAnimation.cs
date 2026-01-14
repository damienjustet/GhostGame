using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAnimation : MonoBehaviour
{
    RectTransform rt;
    public int direction;
    bool animating = false;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        if (gameObject.name == "Game Over Screen")
        {
            Global.Instance.gameOverAnimation = this;
        }
        
    }
    void Update()
    {
        if (!(rt.anchoredPosition.y > 0) && animating && direction == 1)
        {
            rt.anchoredPosition += new Vector2(0, Mathf.Sin(Mathf.PI  * (rt.anchoredPosition.y - 5) / -1200)) * 5000 * Time.unscaledDeltaTime;
        }
        else if (!(rt.anchoredPosition.y < -600) && animating && direction == -1)
        {
            rt.anchoredPosition -= new Vector2(0, Mathf.Sin(Mathf.PI  * (rt.anchoredPosition.y - 5) / -1200)) * 5000 * Time.unscaledDeltaTime;
        }
        else
        {
            animating = false;
        }
    }

    public void ComeHere()
    {
        animating = true;
        direction = 1;
    }

    public void GoAway()
    {
        animating = true;
        direction = -1;
    }
}
