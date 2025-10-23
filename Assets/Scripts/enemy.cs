using System;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using System.Collections.Generic;



//for
//if
//else
//break
//while
//return

enum EnemyState
{
    justSee,
    stalking,
    attack,
    rest,
    dontSee
}

public class EnemyInfo : MonoBehaviour
{

}
public class enemy : MonoBehaviour
{
    float stamina = 0; //Угол нашего круга
    [SerializeField] float maxStamina = 3;
    float attackTime = 999999;
    float restTime = 99999;
    [SerializeField] float radius = 0.5f; //Радиус
    [SerializeField] float speed;
    EnemyState enemyState = EnemyState.dontSee;
    Rigidbody2D rigidbody2D;
    void Start()
    {
        stamina = maxStamina;
        rigidbody2D = GetComponentInChildren<Rigidbody2D>();
    }

    void doSeeSplash(bool bSee)
    {
        //Сделать эффект что мы только что увидели игрока
        if (bSee)
        {

        }
        else
        {
            
        }
    }

    void doAttack()
    {
        //Эффект сделать

    }

    void move()
    {
        print("See");
        if (stamina <= 0 || restTime < 1)
        {
            if (stamina <= 0)
            {
                restTime = 0;
            }
            enemyState = EnemyState.rest;
        }
        else
        {
            enemyState = EnemyState.stalking;
            print("Add force");
            float x = control.Instance.transform.position.x - transform.position.x;
            float y = control.Instance.transform.position.y - transform.position.y;

            float angle = math.atan2(y,x);

            rigidbody2D.AddForce(new Vector2(math.cos(angle), math.sin(angle)) * speed * rigidbody2D.mass * Time.deltaTime);

            stamina -= Time.deltaTime;
            stamina = math.clamp(stamina, 0, maxStamina);
        }        
    }

    void Update()
    {
        float distance = math.sqrt(
            math.pow(transform.position.x - control.Instance.transform.position.x, 2)
        + math.pow(transform.position.y - control.Instance.transform.position.y, 2));

        if (distance <= 6)
        {
            if (enemyState == EnemyState.dontSee)
            {
                enemyState = EnemyState.justSee;
                doSeeSplash(true);
            }


            if (distance <= 1)
            {
                if (attackTime > 1)
                {
                    enemyState = EnemyState.attack;
                    doAttack();
                }
                else
                {
                    move();
                }
            }
            else
            {
                move();
            }
        }
        else
        {
            if (enemyState != EnemyState.dontSee)
            {
                enemyState = EnemyState.dontSee;
                doSeeSplash(false);
            }
        }

        if (enemyState == EnemyState.rest || enemyState == EnemyState.dontSee)
        {
            stamina += Time.deltaTime;
            stamina = math.clamp(stamina, 0, maxStamina);
            restTime += Time.deltaTime;
            restTime = math.clamp(restTime, 0, 99999999);
        }
    }
}