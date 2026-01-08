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

    bool seePlayerSoundEffectPlayed = false;
    bool seePlayer;
    private itemMove cachedItemMove; // Cache to avoid repeated FindObjectOfType calls
    private bool wasPossessed = false; // Track possession state changes

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer; // Initialize timer
    }

    void Update()
    {
        // Clear cache if possession state changed
        if (wasPossessed != LevelLogic.Instance.isPossessed)
        {
            cachedItemMove = null;
            wasPossessed = LevelLogic.Instance.isPossessed;
        }
        
        // Update player reference based on possession state
        if (!LevelLogic.Instance.isPossessed)
        {
            if (player == null || player.name != "player(Clone)")
            {
                player = GameObject.Find("player(Clone)");
                if (player == null)
                {
                    Debug.LogWarning("[ThePOPE] Player object 'player(Clone)' not found!");
                    return;
                }
            }
        }
        else
        {
            // Cache itemMove to avoid repeated FindObjectOfType
            if (cachedItemMove == null)
            {
                cachedItemMove = FindObjectOfType<itemMove>();
            }
            
            if (cachedItemMove != null)
            {
                player = cachedItemMove.gameObject;
            }
            else
            {
                Debug.LogWarning("[ThePOPE] No possessed item found, but isPossessed is true!");
                // Reset cache to try again next frame
                cachedItemMove = null;
                return;
            }
        }
        
        if (player == null)
        {
            Debug.LogWarning("[ThePOPE] Player reference is null!");
            return;
        }
        
        Vector3 direction = player.transform.position - transform.position;
        RaycastHit hit;
        int playerLayer = LayerMask.GetMask("Player", "item");
        Debug.DrawRay(transform.position, direction, Color.blue);
        Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity);
        if (hit.collider.gameObject == player || hit.collider.gameObject.layer == playerLayer)
        {
            seePlayer = true;
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
            if (distanceToPlayer <= wanderRadius && seePlayer)
            {
                if (!seePlayerSoundEffectPlayed)
                {
                    SoundManager.PlaySound(SoundType.POPEFIND);
                    seePlayerSoundEffectPlayed = true;
                }
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
