using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using Cinemachine;
using Unity.VisualScripting;
using Unity.Mathematics;

public class itemMove : MonoBehaviour
{
    public Rigidbody rb;
    CinemachineFreeLook cam;
    public float moveSpeed = 200;
    public float rotationSpeed = 3;
    float yValue;
    float maxVelocity = 5;
    float height;

    private void Awake()
    {
        //references
        cam = GameObject.FindFirstObjectByType<CinemachineFreeLook>();
        rb = gameObject.GetComponent<Rigidbody>();
        
        Collider[] colliders = gameObject.GetComponents<Collider>();
        height = 0;
        foreach (Collider collider in colliders)
        {
            if (collider.bounds.size.x > height)
            {
                height = collider.bounds.size.x;
            }
            if (collider.bounds.size.y > height)
            {
                height = collider.bounds.size.y;
            }
            if (collider.bounds.size.z > height)
            {
                height = collider.bounds.size.z;
            }
        }
        height /= 2;

        rb.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y + 1 + height, rb.transform.position.z);

        CapsuleCollider cp = gameObject.AddComponent<CapsuleCollider>();
        cp.height = (height + 1) / transform.localScale.y;
        cp.radius = 0.001f;
        cp.center = - new Vector3(0, height / transform.localScale.y, 0);

        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized; //move in direction of camera
        Vector3 movement = cameraForward + Camera.main.transform.right; //so you can strafe

        movement *= moveSpeed;
        movement.y = 0;

        rb.velocity = movement * Time.deltaTime;
    }
    void Update()
    {
        //Handles movement
        float y_input = Input.GetAxis("Vertical");
        float x_input = Input.GetAxis("Horizontal");
        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized; //move in direction of camera
        Vector3 movement = cameraForward * y_input + Camera.main.transform.right * x_input; //so you can strafe

        movement *= moveSpeed;        
        movement.y = 0;
        
        rb.velocity = movement * Time.deltaTime;

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
