using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOnCanvas : MonoBehaviour
{
    [Range(0f,100f)] public float canvasWidthPercent;
    [Range(0f,100f)] public float canvasHeightPercent;

    [HideInInspector] public RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

}
