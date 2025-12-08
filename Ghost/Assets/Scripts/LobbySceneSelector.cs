using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbySceneSelector : MonoBehaviour
{
    public string scene;
    public ParticleSystem effect;
    public TextMesh grave;

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
            
            
            
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.name == "player(Clone)")
        {
            effect.Stop();
            grave.color = Color.white;
            interact = false;
        }
    }
    // Start is called before the first frame update

    void Update()
    {
        print(interact);
        if(interact == true && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(scene);
            print("hi");
        }
    }
}
