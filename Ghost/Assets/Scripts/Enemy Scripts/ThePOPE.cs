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

    public GameObject chaseObject;
    
    public Transform exitDoor;

    bool seePlayerSoundEffectPlayed = false;
    bool seePlayer;
    private itemMove cachedItemMove; // Cache to avoid repeated FindObjectOfType calls
    private bool wasPossessed = false; // Track possession state changes

    int playerLayer;

    bool goingToLastSeenPlayer;
    public GameOverAnimation gameOverScreen;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer; // Initialize timer
        player = GameObject.Find("player(Clone)");
        chaseObject = player;
        playerLayer = LayerMask.GetMask("Player", "item");
        Time.timeScale = 5;
    }

    void Update()
    {
        if (LevelLogic.Instance.playerLiving)
        {
            // Clear cache if possession state changed
            if (wasPossessed != LevelLogic.Instance.isPossessed)
            {
                cachedItemMove = null;
                wasPossessed = LevelLogic.Instance.isPossessed;
            }
            
            // Update player reference based on possession state
            if (!LevelLogic.Instance.isPossessed && LevelLogic.Instance.playerLiving)
            {
                chaseObject = player;
            }
            else if (LevelLogic.Instance.playerLiving)
            {
                // Cache itemMove to avoid repeated FindObjectOfType
                if (cachedItemMove == null)
                {
                    cachedItemMove = FindObjectOfType<itemMove>();
                }
                
                if (cachedItemMove != null && chaseObject != cachedItemMove.gameObject)
                {
                    chaseObject = cachedItemMove.gameObject;
                }
                else
                {
                    cachedItemMove = null;
                    return;
                }
            }
            
            Vector3 direction = chaseObject.transform.position - transform.position;
            RaycastHit hit;
            Debug.DrawRay(transform.position, direction, Color.blue);
            Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity);
            if (hit.collider.gameObject == chaseObject)
            {
                seePlayer = true;
            }
            else if (seePlayer)
            {
                goingToLastSeenPlayer = true;
                seePlayer = false;
            }
            float distanceToPlayer = Vector3.Distance(transform.position, chaseObject.transform.position);
            
            timer += Time.deltaTime;
            Transform selfPos = transform;
            
            if (!tired)
            {
                if (distanceToPlayer <= wanderRadius && seePlayer)
                {
                    if (!seePlayerSoundEffectPlayed)
                    {
                        SoundManager.PlaySound(SoundType.POPEFIND);
                        seePlayerSoundEffectPlayed = true;
                    }
                    agent.SetDestination(chaseObject.transform.position);
                }
                else if (!seePlayer && goingToLastSeenPlayer)
                {
                    direction = agent.destination - transform.position;
                    if (Physics.Raycast(transform.position, direction, Vector3.Distance(transform.position, agent.destination), LayerMask.GetMask("Door")) || Vector3.Distance(transform.position, agent.destination) <= 3f)
                    {
                        goingToLastSeenPlayer = false;
                        Debug.Log("Unstuck");
                    }
                }
                else if (timer >= wanderTimer)
                {
                    
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1, selfPos);
                    agent.SetDestination(newPos);
                    timer = 0;
                    
                }
                else
                {
                    if (seePlayerSoundEffectPlayed)
                    {
                        SoundManager.PlaySound(SoundType.POPECURIOUS);
                        seePlayerSoundEffectPlayed = false;
                    }
                }
                
            }
            else
            {
                GameObject exitObj = GameObject.FindGameObjectWithTag("exit");
                if (exitObj != null)
                {
                    exitDoor = exitObj.transform;
                    agent.SetDestination(exitDoor.position);
                }
                else
                {
                    Debug.LogWarning("[ThePOPE] Exit door tagged object not found!");
                }
            }
        }
        else if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1, transform);
            agent.SetDestination(newPos);
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
            
        
    }

    // Function to find a random point on the NavMesh within a sphere
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask, Transform selfTransform)
    {
        while (true)
        {
            Vector3 randomDirection = Random.insideUnitSphere * dist;
            randomDirection += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);
            if (navHit.position != null)
            {
                return navHit.position;
            }
            
        }
        
    }
    
}
