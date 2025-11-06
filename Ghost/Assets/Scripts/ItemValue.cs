// using System.Collections;
// using System.Collections.Generic;
// using System.Numerics;
// using UnityEngine;

// public class ItemValue : MonoBehaviour
// {
//     public float value;
//     [Range(0f, 1f)] public float fragility;
//     [Range(0f, 1f)] public float sensitivity;

//     void Start()
//     {
        
//     }

//     void Update()
//     {

//     }
    
//     // When the object hits something
//     void OnCollisionEnter(Collision collision)
//     {
//         if (GetTotalVelocity(collision.relativeVelocity) > -10 * sensitivity + 20)
//         {
//             if (fragility == 1)
//             {
//                 value = 0;
//             }
//             value -= Round2Decimals(ogValue * fragility * GetMagnitude(collision.relativeVelocity) / velocity.Offset);
//         }
//         if (value < 0)
//         {
//             Destroy(gameObject);
//         }
//     }

//     // Function to get magnitude of velocity
//     float GetMagnitude(UnityEngine.Vector3 collisionVelocity)
//     {
//         return MathF.Sqrt(Math.Pow(collisionVelocity.X, 2) + Math.Pow(collisionVelocity.Y, 2) + Math.Pow(collisionVelocity.Z, 2));
//     }
    
//     // Function to round to 2 decimals
//     float Round2Decimals(float num)
//     {
//         return MathF.Floor(num * 100) / 100;
//     }
// }
