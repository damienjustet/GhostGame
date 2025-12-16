using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCost : MonoBehaviour
{
    public float value;
    [Range(0f, 1f)] public float fragility;
    [Range(0f, 1f)] public float sensitivity;
    posseion possessionScript;

    public GameObject loseValueText;
    public Text shownText;
    bool canDamage = true;


    float ogValue;

    void Start()
    {
        loseValueText = (GameObject)Resources.Load("LoseValueText");
        shownText = loseValueText.GetComponent<Text>();
        ogValue = value;
    }


    float GetVelocityMagnitude(Vector3 collisionVelocity)
    {
        return Mathf.Sqrt(Mathf.Pow(collisionVelocity.x, 2) + Mathf.Pow(collisionVelocity.y, 2) + Mathf.Pow(collisionVelocity.z, 2));
    }
    float Round2Decimals(float num)
    {
        return Mathf.Floor(num * 100) / 100;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (SoundManager.instance.canSound)
        {
            SoundManager.PlaySound(SoundType.ITEMHIT, Mathf.Max(GetVelocityMagnitude(collision.relativeVelocity) / 4, 0.1f));
        }
        
        if (canDamage)
        {
            if (GetVelocityMagnitude(collision.relativeVelocity) > -10 * sensitivity + 10)
            {
                if (fragility == 1)
                {
                    value = 0;
                }
                print(Round2Decimals(ogValue * fragility * GetVelocityMagnitude(collision.relativeVelocity)));
                shownText.text = "-$" + Round2Decimals(ogValue * fragility * GetVelocityMagnitude(collision.relativeVelocity));
                value = Round2Decimals(value - ogValue * fragility * GetVelocityMagnitude(collision.relativeVelocity));
                Instantiate(loseValueText, transform);
            }
            if (value < 0)
            {
                LevelLogic.Instance.interact = false;
                if (LevelLogic.Instance.isPossessed && gameObject.GetComponent<posseion>().item)
                {
                    gameObject.GetComponent<posseion>().Depossess(true);
                }
                Destroy(gameObject);
            }
        }
    }
        
    void Update()
    {
        if (value <= 0 && canDamage)
        {
            Destroy(gameObject);
        }
    }

    public void Collect(Vector3 depossessCoord)
    {
        canDamage = false;
        value = 0;
        gameObject.layer = LayerMask.NameToLayer("Collected Item");
        if (gameObject.GetComponent<itemMove>() != null)
        {
            gameObject.GetComponent<posseion>().Depossess();
            if (gameObject.GetComponent<Rigidbody>() != null)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            Destroy(gameObject.GetComponent<itemMove>());
        }

    }
}
