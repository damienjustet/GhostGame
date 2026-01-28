
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
    bool inSight;
    Transform selfPos;


    void Awake()
    {
        
        
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer; // Initialize timer
        
        // Cache the collectable item at start
        items = GameObject.FindGameObjectsWithTag("Collectable");
        
        item.AddRange(items);
        
        
        
        

        if (item == null)
        {
            Debug.LogWarning($"[RatTargetScript] No Collectable tagged object found on start for {gameObject.name}!");
        }
        
    }

    void Update()
    {   
        
       
            
            
            
            
        
        
        
        timer += Time.deltaTime;
        selfPos = transform;

        if (!scurryAway)
        {
        if (timer >= wanderTimer)
        {

            scanItems();
            
            if (closestItem != null)
                {
                    
                    float distanceToItem2 = Vector3.Distance(transform.position, closestItem.transform.position);
                     if (distanceToItem2 <= wanderRadius)
            {
                
                        
                        print(closestItem + "closest item");
                        agent.SetDestination(closestItem.transform.position);
                        
                   

            }
                   
                }
                 else
                    {
                        
                        agent.SetDestination(RandomNavSphere(transform.position, wanderRadius,-1,selfPos));
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
                GameObject.Find("ratHole").GetComponent<EnemyManager>().RatSpawn = false;
                
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
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);
        
        return navHit.position;
    }

  void scanItems(bool stuck = false)
    {
        items = GameObject.FindGameObjectsWithTag("Collectable");
        item.Clear();
        item.AddRange(items);
        print(closestItem);
            for(int i = 1; i < item.Count; i++)
        {
             Vector3 dir = (transform.position - item[i].transform.position).normalized;
            
            Debug.DrawRay(transform.position, dir * 10, Color.magenta, 1f);
            if(Physics.Raycast(transform.position, dir ,out RaycastHit hit, 50f, 7))
            {
                if(hit.collider.gameObject == item[i])
                {
                    inSight = true;
                    print("hi");
                }
                else
                {
                    inSight = false;
                }
            
            if(closestItem == null && item[i].GetComponent<posseion>().isGrounded && inSight)
            {
                closestItem = item[i];
                
                
            }
            else if(closestItem == null)
            {
                continue;
            }
            
                
            
             float distanceToItem = Vector3.Distance(transform.position, item[i].transform.position);
             float distanceToClosestItem = Vector3.Distance(transform.position, closestItem.transform.position);
            
            
            if(distanceToItem < distanceToClosestItem && item[i].GetComponent<posseion>().isGrounded)
            {
                    closestItem = item[i];
                    
            }
           
            
                    
                }
            
                
                
            }
            
    }}
        
        
    
        
    

