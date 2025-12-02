using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class posseion : MonoBehaviour
{
    public GameObject showValueText;
    Text shownText;
    private bool interactable = false;

    bool inArea;

    RawImage rawImage;

    Vector2 textureCoord;
    [HideInInspector] public Vector3 depossessCoord;
  

    [HideInInspector] public bool item;
    [HideInInspector] public int frame;

    void Start()
    {
        
    }

    public void OnMouseOver1()
    {

        if (!Global.Instance.isPossessed && inArea == true)
        {
            if (gameObject.GetComponent<BoxCollider>() != null)
            {
                showValueText.transform.position = transform.position + new Vector3(0, gameObject.GetComponent<BoxCollider>().size.y / 2, 0);
            }
            else
            {
                showValueText.transform.position = transform.position + new Vector3(0, 0, 0);
            }
            shownText.text = Convert.ToString(gameObject.GetComponent<ItemCost>().value);
            interactable = true;
            GetComponent<Renderer>().material.color = Color.yellow; // Shows if you can click on it. This can be changed for some other effect
            Global.Instance.interact = true; // basically same as interactable var but its so player can access it though don't delete the other one because we need individual vars for the different items
        }
        else
        {
            shownText.text = "";
            
            GetComponent<Renderer>().material.color = Color.white; // Resets color from yellow

            interactable = false;
            Global.Instance.interact = false;
        }

    }
    
    public void OnMouseExit1()
    {
        shownText.text = "";
       
        GetComponent<Renderer>().material.color = Color.white; // Resets color from yellow

        interactable = false;
        Global.Instance.interact = false;
        
    }
    private void Update()
    {
        // I moved the raycast to the global script because it was running an error
        // It will set frame to 0 if it is in the raycast and so it checks here
        // if mouse exits
        
        if (frame == 0)
        {
            frame = 1;
        }
        else if (frame == 1)
        {
            frame = 2;
            item = false;
            OnMouseExit1();
            print("meep");
        }
            
        if (interactable && Input.GetKeyDown(KeyCode.E))
        {
            gameObject.AddComponent<itemMove>();
            

        }
        else if (Input.GetKeyDown(KeyCode.E) && gameObject.GetComponent<CharacterController>() != null) // depossess object
        {
            if (CanDepossess())
            {
                GameObject.FindWithTag("Player").GetComponent<Player>().Depossess(depossessCoord);
                Depossess();
            }
            
        }

    }
    private void OnTriggerEnter(Collider other) // if in area
    {
        
        if (other.gameObject.name == "detectionArea")
        {
            inArea = true;
            
        }

    }
    private void OnTriggerExit(Collider other)// not in area
    {
        if (other.gameObject.name == "detectionArea")
        {
            inArea = false;
            
        }
    }

    public void Depossess()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        Destroy(gameObject.GetComponent<itemMove>());
        gameObject.GetComponent<Rigidbody>().transform.position = gameObject.GetComponent<CharacterController>().transform.position;
        gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<CharacterController>().velocity;
        Destroy(gameObject.GetComponent<CharacterController>());
    }

    public void CreateShownValue()
    {
        rawImage = GameObject.FindObjectOfType<RawImage>();
        showValueText = (GameObject)Resources.Load("ShowValueText");
        showValueText = Instantiate(showValueText, transform);
        showValueText.GetComponent<Transform>().localScale = new Vector3(0.06f,0.06f,0.06f);
        shownText = showValueText.GetComponent<Text>();
        showValueText.GetComponent<ShowValue>().theirParent = gameObject;
    }

    public bool CanDepossess()
    {
        float playerRad = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider>().radius;
        float playerHeight = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider>().height;
        float itemRad = gameObject.GetComponent<Collider>().bounds.size.x / 2;
        
        Vector3 boxSize = new Vector3(playerRad, playerHeight, playerRad);
        for (int i = 1; i <= 4; i++)
        {
            Vector3 moveItemRadius = new Vector3(i % 2 * itemRad, 0, (i - 1.5f) / Mathf.Abs(i - 1.5f) * itemRad);
            Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(i % 2 * playerRad, 0, (i - 1.5f) / Mathf.Abs(i - 1.5f) * playerRad), 2 * boxSize, Quaternion.identity, LayerMask.GetMask("item", "Walls"));
            if (colliders.Length == 0 || colliders.Length == 1)
            {
                depossessCoord = transform.position + moveItemRadius + new Vector3(i % 2 * playerRad, 0, (i - 1.5f) / Mathf.Abs(i - 1.5f) * playerRad);
                return true;
            }
        }
        depossessCoord = transform.position;
        return false;
    }
    void OnDrawGizmosSelected()
    {
        float playerRad = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider>().radius;
        float playerHeight = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider>().height;
        Vector3 boxSize = new Vector3(playerRad, playerHeight, playerRad);
        Gizmos.color = Color.red;
        float itemRad = gameObject.GetComponent<Collider>().bounds.size.x / 2;
        
        Gizmos.DrawWireCube(transform.position + new Vector3(playerRad, 0, playerRad) + new Vector3(itemRad, 0, itemRad), 2 * boxSize);
        Gizmos.DrawWireCube(transform.position + new Vector3(-playerRad, 0, playerRad) + new Vector3(-itemRad, 0, itemRad), 2 * boxSize);
        Gizmos.DrawWireCube(transform.position + new Vector3(playerRad, 0, -playerRad) + new Vector3(itemRad, 0, -itemRad), 2 * boxSize);
        Gizmos.DrawWireCube(transform.position + new Vector3(-playerRad, 0, -playerRad) + new Vector3(-itemRad, 0, -itemRad), 2 * boxSize);
    }

}


