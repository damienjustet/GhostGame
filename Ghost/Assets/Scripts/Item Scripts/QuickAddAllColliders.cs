using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class QuickAddAllColliders : MonoBehaviour
{
    public Material mat;
    void Start()
    {
        
    }

    void Update()
    {
        if (mat != null)
        {
            if (gameObject.GetComponent<Renderer>() != null)
            {
                Renderer mate = gameObject.GetComponent<Renderer>();
                mate.material = mat;
            }
            
            int childCount = gameObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                gameObject.transform.GetChild(i).AddComponent<QuickAddAllColliders>();
                gameObject.transform.GetChild(i).GetComponent<QuickAddAllColliders>().mat = mat;
            }
            DestroyImmediate(gameObject.GetComponent<QuickAddAllColliders>());
        }
    }
    
}
