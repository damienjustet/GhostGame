using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class posseion : MonoBehaviour
{
    public GameObject showValueText;
    Text shownText;
    private bool interactable = false;

    bool inArea;

    RawImage rawImage;

    Vector2 textureCoord;
    [HideInInspector] public Vector3 depossessCoord;
    public bool thisIsPossessed = false;
  

    [HideInInspector] public bool item;
    [HideInInspector] public int frame;
    public void OnMouseOver1()
    {
        if (!LevelLogic.Instance.isPossessed && inArea == true)
        {
            if (gameObject.GetComponent<Collider>() != null)
            {
                showValueText.transform.position = transform.position + new Vector3(0, gameObject.GetComponent<Collider>().bounds.size.y / 2, 0);
            }
            else
            {
                showValueText.transform.position = transform.position + new Vector3(0, 0, 0);
            }
            shownText.text = "$" + Convert.ToString(gameObject.GetComponent<ItemCost>().value);
            interactable = true;
            // GetComponent<Renderer>().material.color = Color.yellow; // Shows if you can click on it. This can be changed for some other effect
            LevelLogic.Instance.interact = true; // basically same as interactable var but its so player can access it though don't delete the other one because we need individual vars for the different items
        }
        else if (!inArea)
        {
            shownText.text = "";
            
            // GetComponent<Renderer>().material.color = Color.white; // Resets color from yellow

            interactable = false;
            LevelLogic.Instance.interact = false;
        }

    }
    
    public void OnMouseExit1()
    {
        shownText.text = "";
       
        // GetComponent<Renderer>().material.color = Color.white; // Resets color from yellow

        interactable = false;
        LevelLogic.Instance.interact = false;
        
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
        }

        if (interactable && Input.GetKeyDown(KeyCode.E))
        {
            gameObject.AddComponent<itemMove>();
            thisIsPossessed = true;
            LevelLogic.Instance.isPossessed = true;
            
        }
        else if (Input.GetKeyDown(KeyCode.E) && thisIsPossessed) // depossess object
        {
            Depossess();
            
        }

        // Set depossession coord to the last depossessable spot
        if (thisIsPossessed)
        {
            CanDepossess();
        }

        Debug.DrawLine(depossessCoord, depossessCoord + Vector3.up);

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

    public void Depossess(bool force = false)
    {
        if (CanDepossess(force))
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().Depossess(depossessCoord);
            if (gameObject.GetComponent<Rigidbody>() != null)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            Destroy(gameObject.GetComponent<itemMove>());
            thisIsPossessed = false;
        }
        
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

    public bool CanDepossess(bool force = false)
    {
        float playerRad = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider>().radius;
        float playerHeight = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider>().height;
        float itemRad = gameObject.GetComponent<Collider>().bounds.size.x / 2;
        
        Vector3 boxSize = new Vector3(playerRad, playerHeight, playerRad) * 2f;
        
        for (int i = 1; i <= 4; i++)
        {
            Vector3 moveItemRadius = new Vector3(((i % 2) - 0.5f) / Mathf.Abs((i % 2) - 0.5f) * itemRad, 0, (i - 2.5f) / Mathf.Abs(i - 2.5f) * itemRad);
            Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(((i % 2) - 0.5f) / Mathf.Abs((i % 2) - 0.5f) * playerRad, 0, (i - 2.5f) / Mathf.Abs(i - 2.5f) * playerRad), 2 * boxSize, Quaternion.identity, LayerMask.GetMask("Walls"));
            if (CollidersAreAll(colliders, gameObject.GetComponent<Collider>().name) || colliders.Length == 0)
            {
                depossessCoord = transform.position + moveItemRadius + new Vector3(i % 2 * playerRad, 0, (i - 2.5f) / Mathf.Abs(i - 2.5f) * playerRad);
                return true;
            }
        }

        return force;

        
    }

    bool CollidersAreAll(Collider[] colliders, string name)
    {
        foreach (Collider col in colliders)
        {
            if (col.name != name)
            {
                return false;
            }
        }
        return true;
    }

    bool BoxCheck(Vector3 center, Vector3 boxSize)
    {
        Collider[] colliders = Physics.OverlapBox(center, boxSize / 2, Quaternion.identity, LayerMask.GetMask("item", "Walls"));
        Debug.DrawLine(center, center + new Vector3(0, 1, 0), Color.green, 3000);
        if (CollidersAreAll(colliders, gameObject.GetComponent<Collider>().name) || colliders.Length == 0)
        {
            depossessCoord = center;
            return true;
        }

        return false;
    }
}


