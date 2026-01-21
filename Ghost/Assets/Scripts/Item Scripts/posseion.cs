using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class posseion : MonoBehaviour
{
    public GameObject showValueText;
    ShowValue valueScript;
    Text shownText;
    private bool interactable = false;

    public bool inArea;

    Vector2 textureCoord;
    [HideInInspector] public Vector3 depossessCoord;
    public bool thisIsPossessed = false;
  

    [HideInInspector] public bool item;
    [HideInInspector] public int frame;

    ItemCost itemCost;
    Collider thisCollider;
    Rigidbody rb;
    GameObject playerObj;
    Player playerScript;
    CapsuleCollider playerCollider;


    public bool isGrounded;

    List<Vector3> depossessableCoords = new List<Vector3>();

    public float maxFloatation;

    void Awake()
    {
        itemCost = gameObject.GetComponent<ItemCost>();
        thisCollider = gameObject.GetComponent<Collider>();
        rb = gameObject.GetComponent<Rigidbody>();

        playerObj = GameObject.Find("player(Clone)");
        playerScript = playerObj.GetComponent<Player>();
        playerCollider = playerObj.GetComponent<CapsuleCollider>();
    }

    public void OnMouseOver1()
    {
        if (!thisIsPossessed && !LevelLogic.Instance.isPossessed && inArea == true)
        {
            interactable = true;
            LevelLogic.Instance.interact = true;
        }
        else if (!inArea)
        {
            interactable = false;
        }

    }
    
    public void OnMouseExit1()
    {
        interactable = false;
        LevelLogic.Instance.interact = false;
        
    }
    private void Update()
    {
        if (!CutsceneManager.Instance.inCutscene)
        {
            showValueText.transform.position = transform.position + new Vector3(0, thisCollider.bounds.size.y / 2 + 0.3F, 0);

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
                maxFloatation = 3;
                itemMove moveComponent = gameObject.AddComponent<itemMove>();
                moveComponent.maxFloatation = maxFloatation;
                thisIsPossessed = true;
                LevelLogic.Instance.isPossessed = true;
                LevelLogic.Instance.interact = false;
                interactable = false;
                playerScript.Possess();
            }
            else if (Input.GetKeyDown(KeyCode.E) && thisIsPossessed) // depossess object
            {
                Depossess();
                
            }

            // Set depossession coord to the last depossessable spot
            // if (thisIsPossessed)
            // {
            //     CanDepossess();
            // }

            Debug.DrawLine(depossessCoord, depossessCoord + Vector3.up);

            if (playerObj != null && thisIsPossessed)
            {
                FindDepossessableCoord();
            }
            RaycastHit hit;
            
            Debug.DrawRay(transform.position, Vector3.down * 1.5f, Color.red, 1000f);
            if (Physics.Raycast(transform.position, Vector3.down * 1.5f, out hit, 1.5f))
            {
                if(hit.collider.gameObject.layer == 10)
                {
                    isGrounded = true;
                    
                }
            }
            else
            {
                isGrounded = false;
            }
        }
            

        }
        private void OnTriggerEnter(Collider other) // if in area
        {
            
            if (other.gameObject.name == "detectionArea")
            {
                inArea = true;
                valueScript.easeDirection = 1;
            }

        }
        private void OnTriggerExit(Collider other)// not in area
        {
            if (other.gameObject.name == "detectionArea")
            {
                inArea = false;
                valueScript.easeDirection = -1;
                
            }
        }

        public void Depossess(bool dead = false)
        {
            depossessCoord = FindDepossessableCoord();
            if (rb != null)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
            
            Destroy(gameObject.GetComponent<itemMove>());

            thisIsPossessed = false;
            LevelLogic.Instance.isPossessed = false;

            if (playerObj != null)
            {
                if (playerScript != null)
                {
                    playerScript.Depossess(depossessCoord);
                }
            }

            if (dead)
            {
                Destroy(gameObject);
            }
        

        //Raycast for Rat
       
    }

    public Vector3 FindDepossessableCoord()
    {
        depossessableCoords.Clear();

        float playerRadius = playerCollider.radius;
        float playerHeight = playerCollider.height;
        float itemBoundsX = thisCollider.bounds.size.x;
        float itemBoundsZ = thisCollider.bounds.size.z;

        Vector3 boxSize = new Vector3(playerRadius * 2, playerHeight, playerRadius * 2);
        Vector3 boxCenter = transform.position;
        boxCenter.y = 0; 

        Ray ray;
        RaycastHit hit;
        Collider[] colliders;
        // Expand out in a grid checking spots
        for (int ring = 0; ring <= 10; ring += 2)
        {
            if (ring != 0)
            {
                for (int side = 1; side <= 4; side++)
                {
                    for (int i = 1; i < ring; i++)
                    {
                        boxCenter.y = transform.position.y;
                        ray = new Ray(boxCenter, Vector3.down);
                        Physics.Raycast(ray, out hit);
                        boxCenter.y = boxSize.y / 2 + hit.point.y + 0.1f;
                        colliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, LayerMask.GetMask("Walls", "item", "Default"));
                        if (colliders.Length == 0)
                        {
                            boxCenter.y = hit.point.y;
                            depossessableCoords.Add(boxCenter);
                            Debug.DrawRay(boxCenter, Vector3.down, Color.green);
                        }
                        else
                        {
                            Debug.DrawRay(boxCenter, Vector3.down, Color.red);
                        }
                        if (side % 2 == 1)
                        {
                            boxCenter.z += boxSize.z * (side - 2);
                        }
                        else
                        {
                            boxCenter.x += boxSize.x * (side - 3);
                        }
                    }
                    boxCenter.y = transform.position.y;
                    ray = new Ray(boxCenter, Vector3.down);
                    Physics.Raycast(ray, out hit);
                    boxCenter.y = boxSize.y / 2 + hit.point.y + 0.1f;
                    colliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, LayerMask.GetMask("Walls", "item", "Default"));
                    if (colliders.Length == 0)
                    {
                        boxCenter.y = hit.point.y;
                        depossessableCoords.Add(boxCenter);   
                        Debug.DrawRay(boxCenter, Vector3.down, Color.green);
                    }
                    else
                    {
                        Debug.DrawRay(boxCenter, Vector3.down, Color.red);
                    }
                    if (side % 2 == 1)
                    {
                        boxCenter.x += boxSize.x * (side - 2);
                    }
                    else if (side == 2)
                    {
                        boxCenter.z += boxSize.z * (-side + 3);
                    }
                }
            }
            else
            {
                if (itemCost.value <= 0)
                {
                    
                    ray = new Ray(transform.position, Vector3.down);
                    Physics.Raycast(ray, out hit);
                    boxCenter.y = boxSize.y / 2 + hit.point.y;
                    colliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, LayerMask.GetMask("Walls", "item", "Default"));
                    if (colliders.Length == 1)
                    {
                        boxCenter.y = hit.point.y;
                        depossessableCoords.Add(boxCenter);   
                        Debug.DrawRay(boxCenter, Vector3.down, Color.green);
                    }
                    else
                    {
                        Debug.DrawRay(boxCenter, Vector3.down, Color.red);
                    }
                }
                
            }

            boxCenter.x += boxSize.x;
            
            
        }
        
        Vector3 target; 
        foreach (Vector3 coord in new List<Vector3>(depossessableCoords))
        {
            target = coord - transform.position;   
            target.y = 0;
            float dist = Mathf.Sqrt(Mathf.Pow(target.x, 2) + Mathf.Pow(target.z, 2));
            if (!Physics.Raycast(transform.position, target, out hit, dist, LayerMask.GetMask("Walls", "Default", "CameraPassthrough")))
            {
                Debug.DrawRay(transform.position, target, Color.blue);
            }
            else
            {
                Debug.DrawRay(transform.position, target, Color.red);
                depossessableCoords.Remove(coord);
            }
        }

        return depossessableCoords[0];
    }

    public void CreateShownValue(GameObject theText)
    {
        showValueText = theText;
        shownText = showValueText.GetComponent<Text>();
        valueScript = showValueText.GetComponent<ShowValue>();
    }


}


