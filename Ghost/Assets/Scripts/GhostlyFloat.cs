using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostlyFloat : MonoBehaviour
{
    [Header("Float Settings")]
    [Tooltip("How fast the object bobs up and down")]
    public float floatSpeed = 2f;
    
    [Tooltip("How far up and down the object moves")]
    public float floatAmount = 0.3f;
    
    [Tooltip("Random offset so multiple objects don't bob in sync")]
    public bool randomizeStart = true;
    
    private Vector3 startPosition;
    private float timeOffset;

    void Start()
    {
        // Store the original position
        startPosition = transform.localPosition;
        
        // Randomize the starting point of the sine wave so objects bob at different times
        if (randomizeStart)
        {
            timeOffset = Random.Range(0f, 2f * Mathf.PI);
        }
        else
        {
            timeOffset = 0f;
        }
    }

    void Update()
    {
        // Calculate the vertical offset using a sine wave
        float yOffset = Mathf.Sin((Time.time * floatSpeed) + timeOffset) * floatAmount;
        
        // Apply the offset to the original position
        transform.localPosition = startPosition + new Vector3(0, yOffset, 0);
    }
}
