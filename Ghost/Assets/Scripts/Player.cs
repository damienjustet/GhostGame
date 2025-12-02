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
    CharacterController rb;
    public GameObject detect;
    public float moveSpeed = 5;
    public CinemachineFreeLook cam;
    [SerializeField] GameObject player;
    public Transform target;

    private void Awake()
    {
        rb = GetComponent<CharacterController>();// finds player
        Cursor.lockState = CursorLockMode.Confined; // confines cursor to window(Might need to click screen to get it to work)
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.Instance.playerLiving)
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

            rb.SimpleMove(movement); // moves player
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
            print(Global.Instance.interact);
            if (Input.GetKeyDown(KeyCode.E) && !Global.Instance.isPossessed && Global.Instance.interact) // Possession
            {
                SoundManager.PlaySound(SoundType.POSSESS);
                Global.Instance.isPossessed = true;
                GameObject.Find("GhostBoi").GetComponent<Renderer>().enabled = false;
                player.GetComponent<Collider>().enabled = false;
                player.GetComponent<CharacterController>().enabled = false;
                detect.GetComponent<Renderer>().enabled = false;
            
               
            }
        }
    }

    public void Depossess(Vector3 pos)
    {
        rb.transform.position = pos;
        gameObject.transform.Find("GhostBoi").GetComponent<Renderer>().enabled = true;
        player.GetComponent<Collider>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        detect.GetComponent<Renderer>().enabled = true;
        Global.Instance.isPossessed = false;
    }

}
