using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using Cinemachine;

public class itemMove : MonoBehaviour
{
    public CharacterController rb;
    CinemachineFreeLook cam;
    public float moveSpeed = 5;
    public float rotationSpeed = 3;
    private void Awake()
    {
        //references
        cam = GameObject.FindFirstObjectByType<CinemachineFreeLook>();
        gameObject.AddComponent<CharacterController>();
        rb = gameObject.GetComponent<CharacterController>();
        rb.height = 2;

        if (gameObject.GetComponent<Rigidbody>() != null) //Allows you to possess rigidbodys
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized; //move in direction of camera
        Vector3 movement = cameraForward + Camera.main.transform.right; //so you can strafe

        movement *= moveSpeed;

        rb.Move(movement * Time.deltaTime);
    }
    void Update()
    {
        //Handles movement
        float y_input = Input.GetAxis("Vertical");
        float x_input = Input.GetAxis("Horizontal");
        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized; //move in direction of camera
        Vector3 movement = cameraForward * y_input + Camera.main.transform.right * x_input; //so you can strafe

        movement *= moveSpeed;

        rb.Move(movement * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Mouse1)) // Rotates possessed object
        {
            
            float mousex = Input.GetAxis("Mouse X");
            float mousey = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up, -mousex * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right, mousey * rotationSpeed, Space.Self);

            cam.m_XAxis.m_MaxSpeed = 0f;// Locks Camera while Rotating
            cam.m_YAxis.m_MaxSpeed = 0f;


        }
        else if(Input.GetKey(KeyCode.Mouse1)) //Unlocks camera if right mouse is held
        {
            cam.m_XAxis.m_MaxSpeed = 300f;
            cam.m_YAxis.m_MaxSpeed = 2f;
        }
    }

}
