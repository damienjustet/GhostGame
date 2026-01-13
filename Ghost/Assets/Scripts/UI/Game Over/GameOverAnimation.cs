using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAnimation : MonoBehaviour
{
    RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (!(rt.anchoredPosition.y > 0))
        {
            rt.anchoredPosition += new Vector2(0, Mathf.Sin(Mathf.PI  * (rt.anchoredPosition.y - 5) / -1200)) * 800 * Time.deltaTime;
        }
    }
}
