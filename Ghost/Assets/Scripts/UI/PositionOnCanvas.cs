using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOnCanvas : MonoBehaviour
{
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

    public void PositionUI(float height, float width)
    {
        if (xAxisAlignment == PositionsX.Left)
        {
            xPos = - width / 2 + xAxisPadding;
        }
        else if (xAxisAlignment == PositionsX.Right)
        {
            xPos = width / 2 - xAxisPadding;
        }
        else
        {
            xPos = xAxisPadding;
        }

        if (yAxisAlignment == PositionsY.Bottom)
        {
            yPos = - height / 2 + yAxisPadding;
        }
        else if (yAxisAlignment == PositionsY.Top)
        {
            yPos = height / 2 - yAxisPadding;
        }
        else
        {
            yPos = yAxisPadding;
        }

        rt.anchoredPosition = new Vector2(xPos, yPos);
    }


}
