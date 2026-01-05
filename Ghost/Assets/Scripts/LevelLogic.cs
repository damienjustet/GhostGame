using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLogic : MonoBehaviour
{
    public static LevelLogic Instance { get; private set; }

    public bool gameIsRunning;

    public float total_time;
    float timer = 0;
    float finish_cooldown;
    public float quota = 5000.00f;
    

    
    SoundManager sm;
    RawImage rawImage;
    
    // MONEY
    public float money;
    public GameObject moneyText;
    Text moneyTextText;
    public Vector2 normalizedPoint;

    
    // checks if ghost is possessed
    public bool isPossessed = false;
    //checks if the player can interact with object(This is for the player script)
    public bool interact;
    
    public bool playerLiving = true;

    // Player health
    public float health = 100;

    public bool canLeave = false;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject cameraDisplay = GameObject.Find("Camera Display");
        if (cameraDisplay != null)
        {
            rawImage = cameraDisplay.GetComponent<RawImage>();
            if (rawImage == null)
            {
                Debug.LogError("[LevelLogic] Camera Display doesn't have a RawImage component!");
            }
        }
        else
        {
            Debug.LogError("[LevelLogic] Camera Display GameObject not found!");
        }
        
        GameObject soundEffects = GameObject.Find("SoundEffects(Clone)");
        if (soundEffects != null)
        {
            sm = soundEffects.GetComponent<SoundManager>();
            if (sm == null)
            {
                Debug.LogError("[LevelLogic] SoundEffects(Clone) doesn't have a SoundManager component!");
            }
        }
        else
        {
            Debug.LogWarning("[LevelLogic] SoundEffects(Clone) GameObject not found!");
        }
        
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        

        if (moneyText != null)
        {
            GameObject canvas = GameObject.Find("Canvas(Clone)");
            if (canvas != null)
            {
                moneyText.transform.SetParent(canvas.transform);
                moneyTextText = moneyText.GetComponent<Text>();
                if (moneyTextText != null)
                {
                    moneyTextText.text = "$0.00";
                }
                else
                {
                    Debug.LogError("[LevelLogic] MoneyText doesn't have a Text component!");
                }
                
                RectTransform rt = moneyText.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchoredPosition = new Vector3(306, 150, 0);
                }
            }
            else
            {
                Debug.LogError("[LevelLogic] Canvas(Clone) GameObject not found!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rawImage == null)
        {
            Debug.LogWarning("[LevelLogic] RawImage is null, cannot perform raycasting!");
            return;
        }
        
        Vector2 localPoint;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition, null, out localPoint))
        {
            normalizedPoint = new Vector2(
            (localPoint.x - rawImage.rectTransform.rect.x) / rawImage.rectTransform.rect.width,
            (localPoint.y - rawImage.rectTransform.rect.y) / rawImage.rectTransform.rect.height
);
            
            if (Camera.main == null)
            {
                Debug.LogError("[LevelLogic] Main Camera not found!");
                return;
            }
            
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(normalizedPoint);
    
            Debug.DrawRay(ray.origin, ray.direction * 10000, Color.green);
            int layerMask = LayerMask.GetMask("item");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {   
                if (hit.collider != null && hit.collider.gameObject != null)
                {
                    posseion ps = hit.collider.gameObject.GetComponentInParent<posseion>();
                    if (ps != null)
                    {
                        ps.item = true;
                        ps.frame = 0;
                        ps.OnMouseOver1();
                    }
                }
            }
            
        }

        
        if (timer > 1)
        {
            SoundManager.instance.canSound = true;
            gameIsRunning = true;
            timer = -1;
        }
        else if (timer >= 0)
        {
            timer += Time.deltaTime;
        }

        string extraMoneyText = "";
        if (money % 1 == 0)
        {
            extraMoneyText = ".00";
        }
        else if (money * 10 % 1 == 0)
        {
            extraMoneyText = "0";
        }
        if (moneyText != null && moneyTextText != null)
        {
            moneyTextText.text = "$" + money + extraMoneyText + "/$" + quota;
            if (money >= quota){
                canLeave = true;
            }
        }
        if (health <= 0)
        {
            Debug.Log("[LevelLogic] Player health reached 0 - You Lose!");
        }
        
    }

    public void UpdateTextPos()
    {
        if (moneyText != null)
        {
            RectTransform rt = moneyText.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = new Vector2(Screen.width / 2 - 3, Screen.height / 2 - 1);
            }
            else
            {
                Debug.LogError("[LevelLogic] UpdateTextPos: MoneyText has no RectTransform!");
            }
        }
        else
        {
            Debug.LogWarning("[LevelLogic] UpdateTextPos: MoneyText is null!");
        }
    }
    
}
