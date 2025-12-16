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
    public float total_time;
    float timer = 0;
    float finish_cooldown;

    
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

    // Start is called before the first frame update
    void Awake()
    {
        rawImage = GameObject.Find("Camera Display").GetComponent<RawImage>();
        sm = GameObject.Find("SoundEffects(Clone)").GetComponent<SoundManager>();
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        MusicType song;
        if (Enum.TryParse(SceneManager.GetActiveScene().name, out song))
        {
            SoundManager.StartSong(song);
        }
        else
        {
            Debug.Log("No music played");
        }

        if (moneyText != null)
        {
            moneyText.transform.SetParent(GameObject.Find("Canvas(Clone)").transform);
            moneyTextText = moneyText.GetComponent<Text>();
            moneyTextText.text = "$0.00";
            moneyText.GetComponent<RectTransform>().anchoredPosition = new Vector3(306,150,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
         Vector2 localPoint;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition, null, out localPoint))
        {
            normalizedPoint = new Vector2(
            (localPoint.x - rawImage.rectTransform.rect.x) / rawImage.rectTransform.rect.width,
            (localPoint.y - rawImage.rectTransform.rect.y) / rawImage.rectTransform.rect.height
);
            RaycastHit hit;
            Ray ray =  Camera.main.ViewportPointToRay(normalizedPoint);
    
            Debug.DrawRay(ray.origin, ray.direction * 10000, Color.green);
            int layerMask = LayerMask.GetMask("item");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {   
                posseion ps = hit.collider.gameObject.GetComponentInParent<posseion>();
                ps.item = true;
                ps.frame = 0;
                ps.OnMouseOver1();
            }
            
        }

        timer += Time.deltaTime;
        if (timer > total_time)
        {
            
        }
        if (finish_cooldown > total_time + finish_cooldown)
        {

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
        if (moneyText != null)
        {
            moneyTextText.text = "$" + money + extraMoneyText;
        }
        if (health <= 0)
        {
            print("You Lose");
        }
        
    }

    public void UpdateTextPos()
    {
        moneyText.GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 2 - 3, Screen.height / 2 - 1);
    }
    
}
