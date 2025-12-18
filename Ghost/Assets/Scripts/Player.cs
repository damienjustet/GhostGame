using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems; 
using Cinemachine;
using UnityEngine.Networking;
using UnityEngine.TextCore.Text;

public class Player : MonoBehaviour
{
    //references
    CharacterController rib;
    public GameObject detect;
    public float moveSpeed = 5;
    public CinemachineFreeLook cam;
    [SerializeField] GameObject player;
    public Transform target;

    private void Awake()
    {
        rib = GetComponent<CharacterController>();// finds player
        Cursor.lockState = CursorLockMode.Confined; // confines cursor to window(Might need to click screen to get it to work)
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))// Locks Cursor for rotating
        {
            Cursor.lockState = CursorLockMode.Locked;
            cam.m_XAxis.m_MaxSpeed = 300f;
            cam.m_YAxis.m_MaxSpeed = 2f;
        }
        else // Unlocks Cursor
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            cam.m_XAxis.m_MaxSpeed = 0f;
            cam.m_YAxis.m_MaxSpeed = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //For Exiting Play to free Mouse
        {
            Cursor.lockState = CursorLockMode.None;
        }

        float y_input = Input.GetAxis("Vertical");
        float x_input = Input.GetAxis("Horizontal");
        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;// Make Camera forawrd the direction you move
        Vector3 movement = cameraForward * y_input + Camera.main.transform.right * x_input;//allows strafing

        movement *= moveSpeed;

        if (rib.enabled)
        {
            rib.SimpleMove(movement); // moves player
        }
        if (Mathf.Abs(y_input) > 0 || Mathf.Abs(x_input) > 0)
        {
            // rotates player
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Mathf.Clamp01(Time.deltaTime * 8));

            // sound effect :)
            SoundManager.StartSound(SoundType.PLAYERMOVE);
        }
        else
        {
            
            SoundManager.StopSound(SoundType.PLAYERMOVE);
        }
    

    }

    public void Depossess(Vector3 pos)
    {
        rib.transform.position = pos;
        
        Transform ghostBoi = gameObject.transform.Find("GhostBoi");
        if (ghostBoi != null)
        {
            Renderer ghostRenderer = ghostBoi.GetComponent<Renderer>();
            if (ghostRenderer != null)
            {
                ghostRenderer.enabled = true;
            }
            else
            {
                Debug.LogError("[Player] Depossess: GhostBoi has no Renderer component!");
            }
        }
        else
        {
            Debug.LogError("[Player] Depossess: GhostBoi child not found!");
        }
        
        if (player != null)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            CharacterController playerController = player.GetComponent<CharacterController>();
            
            if (playerCollider != null)
            {
                playerCollider.enabled = true;
            }
            else
            {
                Debug.LogError("[Player] Depossess: Player Collider not found!");
            }
            
            if (playerController != null)
            {
                playerController.enabled = true;
            }
            else
            {
                Debug.LogError("[Player] Depossess: Player CharacterController not found!");
            }
        }
        else
        {
            Debug.LogError("[Player] Depossess: Player GameObject reference is null!");
        }
        
        if (detect != null)
        {
            Renderer detectRenderer = detect.GetComponent<Renderer>();
            if (detectRenderer != null)
            {
                detectRenderer.enabled = true;
            }
            else
            {
                Debug.LogError("[Player] Depossess: Detect has no Renderer component!");
            }
        }
        else
        {
            Debug.LogError("[Player] Depossess: Detect GameObject reference is null!");
        }
        
        LevelLogic.Instance.isPossessed = false;
        Debug.Log("[Player] Depossessed successfully");
    }

    public void Possess()
    {
        SoundManager.PlaySound(SoundType.POSSESS);
        LevelLogic.Instance.isPossessed = true;
        
        GameObject ghostBoi = GameObject.Find("GhostBoi");
        if (ghostBoi != null)
        {
            Renderer ghostRenderer = ghostBoi.GetComponent<Renderer>();
            if (ghostRenderer != null)
            {
                ghostRenderer.enabled = false;
            }
            else
            {
                Debug.LogError("[Player] Possess: GhostBoi has no Renderer component!");
            }
        }
        else
        {
            Debug.LogError("[Player] Possess: GhostBoi GameObject not found!");
        }
        
        if (player != null)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            CharacterController playerController = player.GetComponent<CharacterController>();
            
            if (playerCollider != null)
            {
                playerCollider.enabled = false;
            }
            else
            {
                Debug.LogError("[Player] Possess: Player Collider not found!");
            }
            
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            else
            {
                Debug.LogError("[Player] Possess: Player CharacterController not found!");
            }
        }
        else
        {
            Debug.LogError("[Player] Possess: Player GameObject reference is null!");
        }
        
        if (detect != null)
        {
            Renderer detectRenderer = detect.GetComponent<Renderer>();
            if (detectRenderer != null)
            {
                detectRenderer.enabled = false;
            }
            else
            {
                Debug.LogError("[Player] Possess: Detect has no Renderer component!");
            }
        }
        else
        {
            Debug.LogError("[Player] Possess: Detect GameObject reference is null!");
        }
        
        Debug.Log("[Player] Possessed successfully");
    }

}
