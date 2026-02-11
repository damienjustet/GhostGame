using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyLoad : MonoBehaviour
{
    public Text text;
    public float money;

    void Awake(){
        money = SaveAndLoadScript.LoadGame();
    }
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        text.text = "$" + money;
    }
}
