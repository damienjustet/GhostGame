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
    
    // SOUNDS
    AudioSource floatSoundSource;
    AudioSource possessSoundSource;

    // Ghost floating bob effect
    public float bobSpeed = 5f;
    public float bobAmount = 0.1f;
    private float bobTimer = 0f;
    Transform ghostBoi;

    [HideInInspector] public bool canMove;

    private void Awake()
    {
        rib = GetComponent<CharacterController>();// finds player
        Cursor.lockState = CursorLockMode.Confined; // confines cursor to window(Might need to click screen to get it to work)

        // Initialize sound sources
        floatSoundSource = gameObject.AddComponent<AudioSource>();
        possessSoundSource = gameObject.AddComponent<AudioSource>();
        SoundManager.environmentSources.Add(floatSoundSource);
        SoundManager.environmentSources.Add(possessSoundSource);
        

        ghostBoi = transform.Find("GhostBoi");
        canMove = true;
    }

    void Start()
    {
        // Get Floatation clip
        floatSoundSource.clip = SoundManager.instance.soundList[(int)SoundType.PLAYERMOVE].clips[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetKey(KeyCode.Mouse1))// Locks Cursor for rotating
            {
                cam.m_XAxis.m_MaxSpeed = 300f;
                cam.m_YAxis.m_MaxSpeed = 2f;
            }
            else // Unlocks Cursor
            {
                cam.m_XAxis.m_MaxSpeed = 0f;
                cam.m_YAxis.m_MaxSpeed = 0f;
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
            
            // Ghost floating bob effect
            if (Mathf.Abs(y_input) > 0 || Mathf.Abs(x_input) > 0)
            {
                bobTimer += Time.deltaTime * bobSpeed;
                float bobOffset = Mathf.Sin(bobTimer) * bobAmount;
                
                // Apply bob to visual model (assuming GhostBoi is the visual)
                if (ghostBoi != null)
                {
                    Vector3 newPos = ghostBoi.localPosition;
                    newPos.y = bobOffset;
                    ghostBoi.localPosition = newPos;
                }
                
                // rotates player
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Mathf.Clamp01(Time.deltaTime * 8));

                // sound effect :)
                if (!floatSoundSource.isPlaying)
                {
                    floatSoundSource.Play();
                }
            }
            else
            {
                // Reset bob when not moving
                if (ghostBoi != null)
                {
                    Vector3 newPos = ghostBoi.localPosition;
                    newPos.y = Mathf.Lerp(newPos.y, 0, Time.deltaTime * 5f);
                    ghostBoi.localPosition = newPos;
                }
                
                floatSoundSource.Stop();
            }
        }
        else
        {
            floatSoundSource.Stop();
            if (!LevelLogic.Instance.playerLiving)
            {
                DestroyImmediate(gameObject);
            }
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
        }
        
        if (player != null)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            CharacterController playerController = player.GetComponent<CharacterController>();
            
            if (playerCollider != null)
            {
                playerCollider.enabled = true;
            }
            
            if (playerController != null)
            {
                playerController.enabled = true;
            }
        }
        
        if (detect != null)
        {
            Renderer detectRenderer = detect.GetComponent<Renderer>();
            if (detectRenderer != null)
            {
                detectRenderer.enabled = true;
            }
        }
        
        LevelLogic.Instance.isPossessed = false;
    }

    public void Possess()
    {
        SoundManager.PlaySoundWithSource(possessSoundSource, SoundType.POSSESS);
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
