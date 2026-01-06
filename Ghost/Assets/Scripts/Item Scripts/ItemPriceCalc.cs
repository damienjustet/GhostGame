using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPriceCalc : MonoBehaviour
{
    GameObject[] items;
    public float totalPrice = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        items = GameObject.FindGameObjectsWithTag("Collectable");
        for(int i = 0; i < items.Length; i++)
        {
            totalPrice += items[i].GetComponent<ItemCost>().value;
            LevelLogic.Instance.quota = totalPrice * 0.5f;
        }

    }}
    


    // Update is called once per frame

