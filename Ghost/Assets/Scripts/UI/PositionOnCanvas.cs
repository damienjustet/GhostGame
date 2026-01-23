using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOnCanvas : MonoBehaviour
{
    [Range(0f,100f)] public float canvasWidthPercent;
    [Range(0f,100f)] public float canvasHeightPercent;

    // Canvas scaling object for positioning
    public CanvasScaling scaler;

    public enum PositionsX
    {
        Right,
        Center,
        Left
    }

    public enum PositionsY
    {
        Top,
        Center,
        Bottom
    }


    [Header("Canvas Positioning")]
    public PositionsX xAxisAlignment;
    public PositionsY yAxisAlignment;

    public float xAxisPadding;
    public float yAxisPadding;

    // Position variables
    float xPos;
    float yPos;

    [HideInInspector] public RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        scaler.uiElementsToPosition.Add(this);
    }

    public void PositionUI()
    {
        if (xAxisAlignment == PositionsX.Left)
        {
            xPos = - Global.Instance.canvasWidth / 2 + xAxisPadding;
        }
        else if (xAxisAlignment == PositionsX.Right)
        {
            xPos = Global.Instance.canvasWidth / 2 - xAxisPadding;
        }
        else
        {
            xPos = xAxisPadding;
        }

        if (yAxisAlignment == PositionsY.Bottom)
        {
            yPos = - Global.Instance.canvasHeight / 2 + yAxisPadding;
        }
        else if (yAxisAlignment == PositionsY.Top)
        {
            yPos = Global.Instance.canvasHeight / 2 - yAxisPadding;
        }
        else
        {
            yPos = yAxisPadding;
        }

        rt.anchoredPosition = new Vector2(xPos, yPos);
    }

#if UNITY_EDITOR

    void OnEnable()
    {
        PositionUI();
    }
#endif

}
