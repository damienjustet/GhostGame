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
    public TextMeshProUGUI textBox;
    public GameObject boxBox;
    public Player player;
    public GameObject eKey;
    [HideInInspector] public bool inCutscene = false;
    int cutsceneNumber = 0;
    int dialogInCutscene = 0;

    float typeSpeed = 25;
    float typeTimer = 0;

    float coolDownTimer = 0;
    
    public Cutscene[] cutscenes;

    

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        boxBox.SetActive(false);
        cm.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (inCutscene && coolDownTimer > 0.2)
        {
            if (Mathf.Floor(typeTimer * typeSpeed) < cutscenes[cutsceneNumber].dialog[dialogInCutscene].words.Length)
            {
                typeTimer += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    typeTimer = cutscenes[cutsceneNumber].dialog[dialogInCutscene].words.Length / typeSpeed;
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
                    CameraToCurrent();
                }
                else if (Input.GetKeyDown(KeyCode.E) && dialogInCutscene == cutscenes[cutsceneNumber].dialog.Length - 1)
                {
                    EndCutscene();
                }
            }
            textBox.text = cutscenes[cutsceneNumber].dialog[dialogInCutscene].words[0..Convert.ToInt32(Mathf.Floor(typeTimer * typeSpeed))];
            
        }
        else
        {
            coolDownTimer += Time.deltaTime;
        }
    }

    public void StartCutscene(int cn)
    {
        cm.enabled = false;
        player.canMove = false;
        textBox.text ="";
        coolDownTimer = 0;
        boxBox.SetActive(true);
        cutsceneNumber = cn;
        dialogInCutscene = 0;
        typeTimer = 0;
        inCutscene = true;
        cutscenes[cutsceneNumber].animation.SetTrigger(cutscenes[cutsceneNumber].parameterName);
        CameraToCurrent();
    }

    public void EndCutscene()
    {
        cutscenes[cutsceneNumber].animation.ResetTrigger(cutscenes[cutsceneNumber].parameterName);
        cutscenes[cutsceneNumber].animation.SetTrigger("Rest");
        player.canMove = true;
        boxBox.SetActive(false);
        inCutscene = false;
        cm.enabled = true;
        eKey.SetActive(false);
    }

    void CameraToCurrent()
    {
        Camera.main.transform.rotation = Quaternion.Euler(cutscenes[cutsceneNumber].dialog[dialogInCutscene].cameraRotation);
        Camera.main.transform.position = cutscenes[cutsceneNumber].dialog[dialogInCutscene].cameraPosition;
    }
}
[Serializable]
public class Cutscene
{
    public Dialogue[] dialog;
    public Animator animation;
    public string parameterName;

    public Cutscene(Dialogue[] words)
    {
        this.dialog = words;
    }
}

[Serializable]
public class Dialogue
{
    public Vector3 cameraPosition;
    public Vector3 cameraRotation;
    [TextArea(3, 10)]
    public string words;
}
