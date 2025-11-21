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
    GameObject tracker;

    void Start()
    {
        showValueText = (GameObject)Resources.Load("ShowValueText");
        showValueText = Instantiate(showValueText, transform);
        shownText = showValueText.GetComponent<Text>();
        showValueText.GetComponent<ShowValue>().theirParent = gameObject;
        rawImage = GameObject.FindObjectOfType<RawImage>();
        tracker = GameObject.Find("tracker");
    }

    private void OnMouseOver()
    {

        if (!Global.Instance.isPossessed && inArea == true)
        {
            showValueText.transform.position = transform.position + new Vector3(0, 0, 0);
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
    
    void OnMouseExit()
    {
        shownText.text = "";
       
        GetComponent<Renderer>().material.color = Color.white; // Resets color from yellow

        interactable = false;
        Global.Instance.interact = false;
    }
    private void Update()
    {
        Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition, null, out localPoint);
        RaycastHit hit;
        Ray ray =  Camera.main.ScreenPointToRay(Input.mousePosition);
        tracker.transform.position = localPoint;
        Debug.DrawRay(ray.origin, ray.direction * 10000, Color.green);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            print("happen");
            OnMouseOver();
        }
      

        if (interactable && Input.GetKeyDown(KeyCode.E))
        {
            gameObject.AddComponent<itemMove>();


        }
        else if (Input.GetKeyDown(KeyCode.E) && gameObject.GetComponent<CharacterController>() != null) // depossess object
        {
            Depossess();
            
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
}


