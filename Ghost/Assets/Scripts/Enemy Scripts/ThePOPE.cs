using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ThePOPE : MonoBehaviour
{
       public float wanderRadius = 20f; // Radius within which the character will wander
    public float wanderTimer = 3f;   // Time before choosing a new destination

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
        
        player = GameObject.Find("player(Clone)");
        Vector3 direction = player.transform.position - transform.position;
        RaycastHit hit;
        int playerLayer = LayerMask.NameToLayer("player");
        Debug.DrawRay(transform.position,direction * 1000f, Color.blue);
        if (Physics.Raycast(transform.position, direction * 1000f, out hit, playerLayer))
        {
            
                seePlayer = true;
                print("hello");
            
            
        }
        else
        {
            seePlayer = false;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        timer += Time.deltaTime;
        Transform selfPos = transform;

        if (!tired)
        {
        if (timer >= wanderTimer)
        {
            if (distanceToPlayer <= wanderRadius && seePlayer)
            {
                agent.SetDestination(player.transform.position);
                

            }
            else
            {

                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1, selfPos);
                agent.SetDestination(newPos);
                
                

            }
            timer = 0;
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
