using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Renderer rendie;
    float timer = 0f;
    Vector3 ogPos;

    // Start is called before the first frame update
    void Start()
    {
        rendie = gameObject.GetComponent<SpriteRenderer>();
        ogPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelLogic.Instance == null)
        {
            Debug.LogError("[Arrow] LevelLogic.Instance is null!");
            return;
        }
        
        if (LevelLogic.Instance.isPossessed && !rendie.enabled)
        {
            rendie.enabled = true;
        }
        else if (!LevelLogic.Instance.isPossessed && rendie.enabled)
        {
            rendie.enabled = false;
        }

        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            Debug.LogWarning("[Arrow] Main Camera not found!");
        }

        timer += Time.deltaTime;

        transform.position = ogPos + Vector3.up * Mathf.Sin(10 * timer) / 10;

    }
}
