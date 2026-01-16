using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }
    public CinemachineBrain cm;
    public CinemachineFreeLook cmf;
    public TextMeshProUGUI textBox;
    public GameObject boxBox;
    public Player player;
    public GameObject eKey;
    bool inCutscene = false;
    int cutsceneNumber = 0;
    int dialogInCutscene = 0;

    float typeSpeed = 15;
    float typeTimer = 0;

    float coolDownTimer = 0;
    
    Cutscene[] cutscenes =  new Cutscene[] 
    {
        new Cutscene(new string[] {"Hello, welcome", "how are you"}),
        new Cutscene(new string[] {"Goodbye", "Mr ghost"}),
        new Cutscene(new string[] {"I'm a kitchen sink you don;t know what that means because a kitchen sink to you is not a kitchen sink to me", "Mr ghost"})
    };

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        boxBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inCutscene && coolDownTimer > 0.2)
        {
            if (Mathf.Floor(typeTimer * typeSpeed) < cutscenes[cutsceneNumber].dialog[dialogInCutscene].Length)
            {
                typeTimer += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    typeTimer = cutscenes[cutsceneNumber].dialog[dialogInCutscene].Length / typeSpeed;
                }
            }
            else
            {
                eKey.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E) && dialogInCutscene < cutscenes[cutsceneNumber].dialog.Length - 1)
                {
                    dialogInCutscene += 1;
                    typeTimer = 0;
                    eKey.SetActive(false);
                }
                else if (Input.GetKeyDown(KeyCode.E) && dialogInCutscene == cutscenes[cutsceneNumber].dialog.Length - 1)
                {
                    EndCutscene();
                }
            }
            textBox.text = cutscenes[cutsceneNumber].dialog[dialogInCutscene][0..Convert.ToInt32(Mathf.Floor(typeTimer * typeSpeed))];
            
        }
        else
        {
            coolDownTimer += Time.deltaTime;
        }
    }

    public void StartCutscene(int cn)
    {
        player.canMove = false;
        textBox.text ="";
        coolDownTimer = 0;
        boxBox.SetActive(true);
        cutsceneNumber = cn;
        dialogInCutscene = 0;
        typeTimer = 0;
        inCutscene = true;
        // cm.enabled = false;
        // Camera.main.transform.position = new Vector3(524.74f, 12.08f, 515.23f);

        Vector3 directionPosition = Vector3.ClampMagnitude(- new Vector3(513.87f, 18.3f, 527.81f) + player.gameObject.transform.position, 1);
        if (directionPosition.z < 0)
        {
            cmf.m_XAxis.Value = - Mathf.Rad2Deg * Mathf.Asin(directionPosition.x);
            Debug.Log("negative x");
        }
        else
        {
            cmf.m_XAxis.Value = 180 + Mathf.Rad2Deg * Mathf.Asin(directionPosition.x);
        }

        print(-new Vector3(513.87f, 18.3f, 527.81f) + player.gameObject.transform.position);
        
        
    }

    public void EndCutscene()
    {
        player.canMove = true;
        boxBox.SetActive(false);
        inCutscene = false;
        cm.enabled = true;
        eKey.SetActive(false);
    }
}

public class Cutscene
{
    public string[] dialog;
    public Cutscene(string[] words)
    {
        this.dialog = words;
    }
}
