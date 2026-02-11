using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbySceneSelector : MonoBehaviour
{
    public string scene;
    public ParticleSystem effect;
    public TextMeshPro grave;

    bool interact;
    void Awake()
    {
        effect.Stop();
    }
   
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name == "player(Clone)")
        {
            
            effect.Play();
            grave.color = Color.green;
            interact = true;
            Global.Instance.InteractKeyChange("E");
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.name == "player(Clone)")
        {
            effect.Stop();
            grave.color = Color.white;
            interact = false;
            Global.Instance.InteractKeyChange("");
        }
    }
    // Start is called before the first frame update

    void Update()
    {
        if(interact == true && Input.GetKeyDown(KeyCode.E))
        {
            if(LevelLogic.Instance != null){
            LevelLogic.Instance.totalMoneys += LevelLogic.Instance.money;
            SaveAndLoadScript.SaveGame(LevelLogic.Instance.totalMoneys);
            SaveAndLoadScript.LoadGame();
            }
           
            Global.Instance.LoadAScene(scene);
        }
    }
}
