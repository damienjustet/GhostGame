using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLogic : MonoBehaviour
{
    public static LevelLogic Instance { get; private set; }

    [HideInInspector] public bool gameIsRunning;

    [HideInInspector] public float total_time;
    float timer = 0;
    float finish_cooldown;
    public float quota = 5000.00f;
    string extraQuotaText;
    public float totalMoneys = 0;

    
    SoundManager sm;
    RawImage rawImage;
    
    // MONEY
    [HideInInspector] public float money;
    [HideInInspector] public GameObject moneyText;
    Text moneyTextText;
    [HideInInspector] public Vector2 normalizedPoint;

    
    // checks if ghost is possessed
    public bool isPossessed = false;
    //checks if the player can interact with object(This is for the player script)
    public bool interact;
    
    public bool playerLiving = true;

    // Player health
    public float health = 100;

    public bool canLeave = false;

    // For GameOver screen
    public bool canGameOver;

    GameOverScreen goScreen;

    // For pope targeting so no GameObject.Find() :)
    public itemMove floatyPopeTarget;

    // Start is called before the first frame update
    void Awake()
    {
        rawImage = GameObject.Find("Camera Display").GetComponent<RawImage>();
        sm = GameObject.Find("SoundEffects(Clone)").GetComponent<SoundManager>();
        
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
                    rt.anchoredPosition = new Vector3(290, 150, 0);
                }
            }
            else
            {
                Debug.LogError("[LevelLogic] Canvas(Clone) GameObject not found!");
            }


        }

        if (quota % 1 == 0)
        {
            extraQuotaText = ".00";
        }
        else if (quota * 10 % 1 == 0)
        {
            extraQuotaText = "0";
        }
        else
        {
            extraQuotaText = "";
        }
        
        if (canGameOver)
        {
            goScreen = GameObject.Find("Game Over Canvas").GetComponent<GameOverScreen>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canGameOver)
        {
            if (!goScreen.end && !playerLiving)
            {
                SoundManager.StartSong(MusicType.DEAD);
                goScreen.GameOver();
            }
        }
        

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
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {   
                if (hit.collider != null && hit.collider.gameObject != null)
                {
                    if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "item")
                    {
                        posseion ps = hit.collider.gameObject.GetComponentInParent<posseion>();
                        if (ps != null)
                        {
                            ps.item = true;
                            ps.frame = 0;
                            ps.OnMouseOver1();
                            if (!isPossessed && ps.inArea)
                            {
                                Global.Instance.InteractKeyChange("E");
                            }
                        }
                    }
                    else if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Door")
                    {
                        GameObject doorObject = hit.collider.gameObject;
                        DoorHinge hingeScript = doorObject.GetComponentInParent<DoorHinge>();
                        if (hingeScript != null && !isPossessed && hingeScript.inArea)
                        {
                            
                            Global.Instance.InteractKeyChange("E");
                            
                            if (hingeScript.inArea && Input.GetKeyDown(KeyCode.E))
                            {
                                hingeScript.DoorInteract();
                            }
                        }
                    }
                    else
                    {
                        Global.Instance.InteractKeyChange("");
                    }
                }
                
            }
            else
            {
                Global.Instance.InteractKeyChange("");
            }
            
        }
        else
        {
            print("failure");
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
            moneyTextText.text = "$" + money + extraMoneyText + "/$" + quota + extraQuotaText;
            if (money >= quota){
                canLeave = true;
            }
        }
        
    }

    public void UpdateTextPos()
    {
        if (moneyText != null)
        {
            RectTransform rt = moneyText.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = new Vector2(Screen.width / 2 - 10, Screen.height / 2 - 1);
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
