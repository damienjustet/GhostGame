using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class CreatePossessable : MonoBehaviour
{

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("item");
        gameObject.tag = "Collectable";
        
        gameObject.AddComponent<ItemCost>();
        gameObject.AddComponent<posseion>();
        gameObject.AddComponent<Rigidbody>();
        DestroyImmediate(gameObject.GetComponent<CreatePossessable>());
    }
}
