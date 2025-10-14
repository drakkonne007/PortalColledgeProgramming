// using System;
// using UnityEngine;

// public class GameState : MonoBehaviour
// {
//     public static GameState Instance { get; private set; }
//     void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }
//         Instance = this;
//     }



//     public float Health()
//     {
//         return health;
//     }

//     public void SetHealth(float hp)
//     {
//         //Вот здесь у нас много всякой логики
//         health += hp;
//         health = Math.Clamp(health, 0, healthMax);
//     }

//     public float Stamina()
//     {
//         return stamina;
//     }

//     public void SetStamina(float stam)
//     {
//         //Вот здесь у нас много всякой логики
//         stamina += stam;
//         stamina = Math.Clamp(stamina, 0, staminaMax);
//     }

//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
