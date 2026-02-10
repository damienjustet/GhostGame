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
    Collider col;
    CinemachineFreeLook cam;
    public float moveSpeed = 4;
    public float rotationSpeed = 2;
    float yValue;
    float maxVelocity = 0.01f;
    float height;

    Vector3 goalMaxFloatation;
    public float maxFloatation;
    bool atMaxFloatation;

    // Axis inputs
    float y_input = 0;
    float x_input = 0;
    float y_direction = 0;

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
                col = collider;
            }
            if (collider.bounds.size.y > height)
            {
                height = collider.bounds.size.y;
                col = collider;
            }
            if (collider.bounds.size.z > height)
            {
                height = collider.bounds.size.z;
                col = collider;
            }
        }
        height /= 2;

        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized; //move in direction of camera
        Vector3 movement = cameraForward + Camera.main.transform.right; //so you can strafe

        movement *= moveSpeed;
        movement.y = 0;
        Vector3.ClampMagnitude(movement, 1f);

        // rb.velocity = movement * Time.deltaTime;
        rb.AddForce(movement * maxVelocity - maxVelocity * rb.velocity, ForceMode.VelocityChange);
    }
    void Update()
    {
        //Handles movement
        if (!CutsceneManager.Instance.inCutscene)
        {
            y_input = Input.GetAxis("Vertical");
            x_input = Input.GetAxis("Horizontal");
            y_direction = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                y_direction = 1;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                y_direction = -1;
            }
        }
        else
        {
            y_input = 0;
            x_input = 0;
            y_direction = 0; 
        }

        rb.angularVelocity = new Vector3(0,0,0);

        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized; //move in direction of camera
        Vector3 movement = cameraForward * y_input + Camera.main.transform.right * x_input; //so you can strafe

        movement.y = y_direction;
        movement *= moveSpeed;    

        // No flying off into the distance
        CheckDown(maxFloatation);   
        if (atMaxFloatation && goalMaxFloatation.y < transform.position.y)
        {
            movement.y = -5;
        }
        else if (atMaxFloatation && goalMaxFloatation.y >= transform.position.y)
        {
            movement.y = 0;
        }
        
        Vector3.ClampMagnitude(movement, 1f);
        
        // rb.velocity = movement * Time.deltaTime;
        rb.AddForce(movement * maxVelocity - maxVelocity * rb.velocity, ForceMode.VelocityChange);

        if (Input.GetKey(KeyCode.Mouse0)) // Rotates possessed object
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked; // Lock cursor to prevent hitting screen edge
            
            float mousex = Input.GetAxis("Mouse X");
            float mousey = Input.GetAxis("Mouse Y");
            
            transform.Rotate(Vector3.up, -mousex * rotationSpeed, Space.World);

            Collider collider = GetComponent<Collider>();
            if (!Physics.BoxCast(transform.position + Vector3.up * (collider.bounds.size.y / 2 - 0.1f), new Vector3(collider.bounds.size.x, 0.01f, collider.bounds.size.z) / 2, Vector3.up, Quaternion.identity, 0.1f) && !Physics.BoxCast(transform.position - Vector3.up * (collider.bounds.size.y / 2 - 0.1f), new Vector3(collider.bounds.size.x, 0.01f, collider.bounds.size.z) / 2, Vector3.down, Quaternion.identity, 0.2f))
            {
                transform.RotateAround(transform.position, Camera.main.transform.right, mousey * rotationSpeed);
            }
            Debug.DrawLine(transform.position + Vector3.up * (collider.bounds.size.y / 2 - 0.1f), transform.position + Vector3.up * (collider.bounds.size.y / 2 + 0.1f) + Vector3.up * 0.1f);
            Debug.DrawLine(transform.position - Vector3.up * (collider.bounds.size.y / 2 - 0.1f), transform.position - Vector3.up * (collider.bounds.size.y / 2 + 0.1f) - Vector3.up * 0.1f);
            cam.m_XAxis.m_MaxSpeed = 0f;// Locks Camera while Rotating
            cam.m_YAxis.m_MaxSpeed = 0f;


        }
        else if(Input.GetKey(KeyCode.Mouse1)) //Unlocks camera if right mouse is held
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked; // Lock cursor for camera rotation
            cam.m_XAxis.m_MaxSpeed = 300f;
            cam.m_YAxis.m_MaxSpeed = 2f;
        }
        else // Release cursor when not rotating
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            UnityEngine.Cursor.visible = true;
        }

        
    }

    void CheckDown(float dist)
    {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, col.bounds.size / 2, Vector3.down, out hit, Quaternion.identity, LayerMask.GetMask("Default", "item", "ground")))
        {
            if (transform.position.y - dist >= hit.point.y)
            {
                goalMaxFloatation = new Vector3(transform.position.x, hit.point.y + dist, transform.position.z);
                
                if (transform.position.y - goalMaxFloatation.y < 0.2f)
                {
                    transform.position = goalMaxFloatation;
                    atMaxFloatation = false;
                }
                else
                {
                    atMaxFloatation = true;
                }
                
            }
            else
            {
                atMaxFloatation = false;
            }
        }
    }

}
