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
    float yValue;

    private void Awake()
    {
        //references
        cam = GameObject.FindFirstObjectByType<CinemachineFreeLook>();
        gameObject.AddComponent<CharacterController>();
        rb = gameObject.GetComponent<CharacterController>();
        
        // if (gameObject.GetComponent<Collider>() != null)
        // {
        //     rb.height = gameObject.GetComponent<Collider>().bounds.size.y / transform.localScale.y;
        //     rb.radius = gameObject.GetComponent<Collider>().bounds.size.x / transform.localScale.x / 2;
        // }
        // else
        // {
        //     rb.height = 2;
        //     rb.radius = 0.5f;
        // }
        rb.height = 0;
        rb.radius = 0;
        yValue = transform.position.y + rb.height/2 + 1;

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
        rb.transform.position = new Vector3(rb.transform.position.x, yValue, rb.transform.position.z);

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Mouse1)) // Rotates possessed object
        {
            
            float mousex = Input.GetAxis("Mouse X");
            float mousey = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up, -mousex * rotationSpeed, Space.World);
            transform.RotateAround(transform.position, Camera.main.transform.right, mousey * rotationSpeed);
            
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
