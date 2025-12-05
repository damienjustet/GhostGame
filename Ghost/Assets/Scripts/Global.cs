using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Global : MonoBehaviour
{
    //This makes script public
    public static Global Instance { get; private set; }
    public bool playerLiving = true;
    RawImage rawImage;

    SoundManager sm;
    public GameObject moneyText;
    Text moneyTextText;
    
    // MONEY
    public float money;

    public float health = 100;

    private void Awake()
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
        }
        
        
    }

    void Update()
    {
        Vector2 localPoint;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition, null, out localPoint))
        {
            Vector2 normalizedPoint = new Vector2(
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

    public void StartGame()
    {
        SceneManager.LoadScene("LOBBY");
    }

    // checks if ghost is possessed
    public bool isPossessed = false;

    //checks if the player can interact with object(This is for the player script)
    public bool interact;

}

