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
    public float moveSpeed = 4;
    public float rotationSpeed = 50;
    float yValue;
    float maxVelocity = 0.01f;
    float height;
    quaternion initRotation;
    Vector2 previousMousePosition;

    private void Awake()
    {
        //references
        cam = GameObject.FindFirstObjectByType<CinemachineFreeLook>();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        
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

        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized; //move in direction of camera
        Vector3 movement = cameraForward + Camera.main.transform.right; //so you can strafe

        movement *= moveSpeed;
        movement.y = 0;
        Vector3.ClampMagnitude(movement, 1f);
        initRotation = rb.rotation;

        // rb.velocity = movement * Time.deltaTime;
        rb.AddForce(movement * maxVelocity - maxVelocity * rb.velocity, ForceMode.VelocityChange);
        previousMousePosition = LevelLogic.Instance.normalizedPoint;
        rb.maxAngularVelocity = 10f;
        
    }
    void FixedUpdate()
    {
        //Handles movement
        float y_input = Input.GetAxis("Vertical");
        float x_input = Input.GetAxis("Horizontal");
        float y_direction = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            y_direction = 1;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            y_direction = -1;
        }

        rb.angularVelocity = new Vector3(0,0,0);

        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized; //move in direction of camera
        Vector3 movement = cameraForward * y_input + Camera.main.transform.right * x_input; //so you can strafe

        movement.y = y_direction;
        movement *= moveSpeed;        
        
        Vector3.ClampMagnitude(movement, 1f);
        
        // rb.velocity = movement * Time.deltaTime;
        rb.AddForce(movement * maxVelocity - maxVelocity * rb.velocity, ForceMode.VelocityChange);

        if (Input.GetKey(KeyCode.Mouse0)) // Rotates possessed object
        {
           
            Vector3 mouseDelta = (LevelLogic.Instance.normalizedPoint - previousMousePosition) * 4;

        
        float torqueX = -mouseDelta.x * rotationSpeed; 
        float torqueY = -mouseDelta.y * rotationSpeed; 
        
        Vector3 camNormal = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up);
        
        // Apply torque
        rb.AddRelativeTorque(camNormal * torqueY, ForceMode.VelocityChange);
        rb.AddRelativeTorque(transform.up * torqueX, ForceMode.VelocityChange);
        
        // Update the previous mouse position for the next frame
            previousMousePosition = LevelLogic.Instance.normalizedPoint;
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
