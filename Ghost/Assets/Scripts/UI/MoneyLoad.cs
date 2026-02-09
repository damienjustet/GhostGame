using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyLoad : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        float money = SaveAndLoadScript.LoadGame();
        text.text = "$" + money;
    }
}
