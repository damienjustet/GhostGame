using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shop;
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            //openShop();
        }
    }

   /* public void openShop(){
        shop.visible = true;
    }
    public void closeShop(){
        shop.visible = false;
    }
    */
}
