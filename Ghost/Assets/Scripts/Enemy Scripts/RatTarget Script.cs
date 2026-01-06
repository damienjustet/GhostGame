using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;


public class RatTargetScript : MonoBehaviour
{
    public float wanderRadius = 10f; // Radius within which the character will wander
    public float wanderTimer = 2f;   // Time before choosing a new destination

    private NavMeshAgent agent;
    private float timer;
    public GameObject[] items;
    public GameObject closestItem;
    public bool scurryAway = false;
    int waitTime;
    public Transform ratHole;
    GameObject currentItem;
    public List<GameObject> item = new List<GameObject>();
    bool stuck = false;


    void Start()
    {
        closestItem = GameObject.FindGameObjectWithTag("Collectable");
        
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer; // Initialize timer
        
        // Cache the collectable item at start
        items = GameObject.FindGameObjectsWithTag("Collectable");
        
        item.AddRange(items);
        
        

        if (item == null)
        {
            Debug.LogWarning($"[RatTargetScript] No Collectable tagged object found on start for {gameObject.name}!");
        }
        scanItems();
    }

    void Update()
    {
        // Only search for item if we don't have one
        
        if (closestItem == null){
            scanItems();
        }
        
        
        float distanceToItem2 = Vector3.Distance(transform.position, closestItem.transform.position);
        timer += Time.deltaTime;
        Transform selfPos = transform;

        if (!scurryAway)
        {
        if (timer >= wanderTimer)
        {
            if (distanceToItem2 <= wanderRadius)
            {
                
               
                        agent.SetDestination(closestItem.transform.position);
                        
                   

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
            GameObject ratHoleObj = GameObject.FindGameObjectWithTag("ratHole");
            if (ratHoleObj != null)
            {
                ratHole = ratHoleObj.transform;
                agent.SetDestination(ratHole.position);
            }
            else
            {
                Debug.LogWarning("[RatTargetScript] Rat hole tagged object not found!");
            }
        }
        
    }

    // Function to find a random point on the NavMesh within a sphere
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask, Transform selfTransform)
    {

        Vector3 randomDirection = Random.insideUnitSphere * dist;
       //Vector3 playerToItem = item1.transform.position - selfTransform.position;
        randomDirection += origin;
        //randomDirection -= playerToItem;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);
        return navHit.position;
    }

  void scanItems(bool stuck = false)
    {
        for(int i = 1; i < item.Count; i++)
        {
             float distanceToItem = Vector3.Distance(transform.position, item[i].transform.position);
              float distanceToClosestItem = Vector3.Distance(transform.position, closestItem.transform.position);
              Vector3 direction = item[i].transform.position - transform.position;
              RaycastHit hit;
            
            if(distanceToItem < distanceToClosestItem)
            {
                if (Physics.Raycast(transform.position, direction.normalized, out hit))
            {
                if (hit.collider.gameObject == item[i])
                {
                    closestItem = item[i];
                }
            }
            
                else if (stuck)
                {
                    print("stuck");
                    item.Remove(item[i]);
                    continue;
                }
                
            }
        }
    }
}
