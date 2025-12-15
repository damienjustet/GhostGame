using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ThePOPE : MonoBehaviour
{
       public float wanderRadius = 20f; // Radius within which the character will wander
    public float wanderTimer = 2f;   // Time before choosing a new destination

    private NavMeshAgent agent;
    private float timer;
    public GameObject player;
    public bool tired = false;
    
    public Transform exitDoor;

bool seePlayer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer; // Initialize timer
    }

    void Update()
    {
        if (!LevelLogic.Instance.isPossessed)
        {
            player = GameObject.Find("player(Clone)");
        }
        else
        {
           if (FindObjectOfType<itemMove>() != null)
            {
                player = FindObjectOfType<itemMove>().gameObject;
            }
            
            
           
        }
         Vector3 direction = player.transform.position - transform.position;
            RaycastHit hit;
            int playerLayer = LayerMask.GetMask("player");
            Debug.DrawRay(transform.position,direction, Color.blue);
            seePlayer = Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity);
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
         timer += Time.deltaTime;
        Transform selfPos = transform;
        
        
        if (!tired)
        {
            if (distanceToPlayer <= wanderRadius && seePlayer)
            {
                if (hit.collider.gameObject.name == "player(Clone)" && !LevelLogic.Instance.isPossessed)
                {
                    agent.SetDestination(player.transform.position);
                }
                else if(hit.collider.gameObject.tag == "Collectable")
                {
                    agent.SetDestination(player.transform.position);
                }
                
                

            }
            else if (timer >= wanderTimer)
        {
            
            
            {
                
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1, selfPos);
                agent.SetDestination(newPos);
                timer = 0;
                

            }
            
        }
            
        }
        else
            {
                exitDoor = GameObject.FindGameObjectWithTag("exit").transform;
                agent.SetDestination(exitDoor.position);
            }
        
    }

    // Function to find a random point on the NavMesh within a sphere
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask, Transform selfTransform)
    {

        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);
        return navHit.position;
    }
    
}
