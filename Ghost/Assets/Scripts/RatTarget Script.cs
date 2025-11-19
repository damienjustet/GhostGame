using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;


public class RatTargetScript : MonoBehaviour
{
    public float wanderRadius = 10f; // Radius within which the character will wander
    public float wanderTimer = 2f;   // Time before choosing a new destination

    private NavMeshAgent agent;
    private float timer;
    public GameObject item;

    int tiredness = 0;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer; // Initialize timer
    }

    void Update()
    {
        item = GameObject.FindWithTag("Collectable");
        float distanceToItem = Vector3.Distance(transform.position, item.transform.position);
        timer += Time.deltaTime;
        Transform selfPos = transform;
          

        if (timer >= wanderTimer)
        {
            if (distanceToItem <= wanderRadius)
            {
                agent.SetDestination(item.transform.position);
                

            }
            else
            {

                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1, item, selfPos);
                agent.SetDestination(newPos);
                
                

            }
            timer = 0;
        }
        
    }

    // Function to find a random point on the NavMesh within a sphere
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask, GameObject item1, Transform selfTransform)
    {

        Vector3 randomDirection = Random.insideUnitSphere * dist;
       //Vector3 playerToItem = item1.transform.position - selfTransform.position;
        randomDirection += origin;
        //randomDirection -= playerToItem;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);
        return navHit.position;
    }
    
}
