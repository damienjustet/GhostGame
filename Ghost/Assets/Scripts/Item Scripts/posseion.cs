using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.Mathematics;
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

    public float maxFloatation;
    public void OnMouseOver1()
    {
        if (!thisIsPossessed && !LevelLogic.Instance.isPossessed && inArea == true)
        {
            if (showValueText == null || shownText == null)
            {
                Debug.LogWarning($"[posseion] ShowValueText or Text component is null on {gameObject.name}. Call CreateShownValue() first.");
                return;
            }
            
            ItemCost itemCost = gameObject.GetComponent<ItemCost>();
            if (itemCost == null)
            {
                Debug.LogError($"[posseion] ItemCost component missing on {gameObject.name}");
                return;
            }
            
            if (gameObject.GetComponent<Collider>() != null)
            {
                showValueText.transform.position = transform.position + new Vector3(0, gameObject.GetComponent<Collider>().bounds.size.y / 2, 0);
            }
            else
            {
                showValueText.transform.position = transform.position + new Vector3(0, 0, 0);
            }
            shownText.text = "$" + Convert.ToString(itemCost.value);
            interactable = true;
            LevelLogic.Instance.interact = true;
        }
        else if (!inArea)
        {
            if (shownText != null)
            {
                shownText.text = "";
            }
            interactable = false;
        }

    }
    
    public void OnMouseExit1()
    {
        if (shownText != null)
        {
            shownText.text = "";
        }

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
            GameObject playerObj = GameObject.Find("player(Clone)");
            if (playerObj == null)
            {
                Debug.LogError("[posseion] Player object 'player(Clone)' not found! Cannot possess.");
                return;
            }
            
            Player playerScript = playerObj.GetComponent<Player>();
            if (playerScript == null)
            {
                Debug.LogError("[posseion] Player component missing on player object!");
                return;
            }
            
            maxFloatation = 3;
            itemMove moveComponent = gameObject.AddComponent<itemMove>();
            moveComponent.maxFloatation = maxFloatation;
            thisIsPossessed = true;
            LevelLogic.Instance.isPossessed = true;
            LevelLogic.Instance.interact = false;
            interactable = false;
            playerScript.Possess();
            
            Debug.Log($"[posseion] Possessed {gameObject.name}");
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
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
            
            itemMove moveComp = gameObject.GetComponent<itemMove>();
            if (moveComp != null)
            {
                Destroy(moveComp);
            }
            
            thisIsPossessed = false;
            LevelLogic.Instance.isPossessed = false;
            
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                Player playerScript = playerObj.GetComponent<Player>();
                if (playerScript != null)
                {
                    playerScript.Depossess(depossessCoord);
                    Debug.Log($"[posseion] Depossessed {gameObject.name}");
                }
                else
                {
                    Debug.LogError("[posseion] Player component not found on Player tagged object!");
                }
            }
            else
            {
                Debug.LogError("[posseion] Player tagged object not found! Cannot depossess properly.");
            }
        }
        else
        {
            Debug.LogWarning($"[posseion] Cannot depossess {gameObject.name} - no valid position found!");
        }
        
        if (force)
        {
            Debug.Log($"[posseion] Force destroying {gameObject.name}");
            Destroy(gameObject);
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
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("[posseion] CanDepossess: Player tagged object not found!");
            return force;
        }
        
        CapsuleCollider playerCollider = playerObj.GetComponent<CapsuleCollider>();
        if (playerCollider == null)
        {
            Debug.LogError("[posseion] CanDepossess: CapsuleCollider not found on player!");
            return force;
        }
        
        Collider itemCollider = gameObject.GetComponent<Collider>();
        if (itemCollider == null)
        {
            Debug.LogError($"[posseion] CanDepossess: No collider on {gameObject.name}!");
            return force;
        }
        
        float playerRad = playerCollider.radius;
        float playerHeight = playerCollider.height;
        float itemWidth = itemCollider.bounds.size.x / 2;
        float itemLength = itemCollider.bounds.size.z / 2;

        Vector3 boxSize = new Vector3(playerRad, playerHeight, playerRad);
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, LayerMask.GetMask("Default", "item"));

        // Positive z side
        for (float i = -Mathf.Floor(itemWidth / (2 * playerRad)); i <= Mathf.Floor(itemWidth / (playerRad * 2)); i++)
        {
            Vector3 center = transform.position + new Vector3(i * 2 *playerRad, 0, itemLength / 2 + 2 * playerRad);
            center.y = hit.point.y + boxSize.y / 2;
            Collider[] colliders = Physics.OverlapBox(center, boxSize, quaternion.identity, LayerMask.GetMask("Walls", "item", "Default"));
            if (colliders.Length == 0)
            {
                depossessCoord = center;
                return true;
            }

            Debug.DrawLine(center + Vector3.down, center + Vector3.up);
        }

        //negative z side
        for (float i = -Mathf.Floor(itemWidth / (2 * playerRad)); i <= Mathf.Floor(itemWidth / (2 * playerRad)); i++)
        {
            Vector3 center = transform.position + new Vector3(i * 2 * playerRad, 0, -(itemLength / 2) - (2 * playerRad));
            center.y = hit.point.y + boxSize.y / 2;
            Collider[] colliders = Physics.OverlapBox(center, boxSize, quaternion.identity, LayerMask.GetMask("Walls", "item", "Default"));
            if (colliders.Length == 0)
            {
                depossessCoord = center;
                return true;
            }

            Debug.DrawLine(center + Vector3.down, center + Vector3.up);
        }

        // Positive x side
        for (float i = -Mathf.Floor(itemLength / (2 * playerRad)); i <= Mathf.Floor(itemLength / (2 * playerRad)); i++)
        {
            Vector3 center = transform.position + new Vector3(itemWidth / 2 + 2 * playerRad, 0, i * 2 *playerRad);
            center.y = hit.point.y + boxSize.y / 2;
            Collider[] colliders = Physics.OverlapBox(center, boxSize, quaternion.identity, LayerMask.GetMask("Walls", "item", "Default"));
            if (colliders.Length == 0)
            {
                depossessCoord = center;
                return true;
            }

            Debug.DrawLine(center + Vector3.down, center + Vector3.up);
        }

        //negative x side
        for (float i = -Mathf.Floor(itemLength / (2 * playerRad)); i <= Mathf.Floor(itemLength / (2 * playerRad)); i++)
        {
            Vector3 center = transform.position + new Vector3(-(itemWidth / 2 + 2 * playerRad), 0, i * 2 *playerRad);
            center.y = hit.point.y + boxSize.y / 2;
            Collider[] colliders = Physics.OverlapBox(center, boxSize, quaternion.identity, LayerMask.GetMask("Walls", "item", "Default"));
            if (colliders.Length == 0)
            {
                depossessCoord = center;
                return true;
            }

            Debug.DrawLine(center + Vector3.down, center + Vector3.up);
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


