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
        SoundManager.PlaySound(SoundType.ITEMHIT, Mathf.Max(GetVelocityMagnitude(collision.relativeVelocity) / 4, 0.1f));

        if (GetVelocityMagnitude(collision.relativeVelocity) > -10 * sensitivity + 10)
        {
            if (fragility == 1)
            {
                value = 0;
            }

            shownText.text = "-" + Round2Decimals(ogValue * fragility * GetVelocityMagnitude(collision.relativeVelocity));
            value = Round2Decimals(value - ogValue * fragility * GetVelocityMagnitude(collision.relativeVelocity));
            // loseValueText.GetComponent<LoseValueWorldToScreenPoint>().place = transform.position;
            Instantiate(loseValueText, transform);
        }
        if (value < 0)
        {
            Global.Instance.interact = false;
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (value <= 0)
        {
            Destroy(gameObject);
        }
    }
}
