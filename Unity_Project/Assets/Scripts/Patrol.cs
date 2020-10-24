using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Patrol : Enemy
{
    public Transform[] patrolPoints;
    public float speed;
    int currentPointIndex;

    private float waitTime;
    public float startWaitTIme;

    private Animator anim;

    //public int damage;
    private void Start()
    {
        transform.position = patrolPoints[0].position;
        transform.rotation = patrolPoints[0].rotation;
        waitTime = startWaitTIme;
        anim = GetComponent<Animator>();
        
    }

    private void Update(){
        transform.position = Vector2.MoveTowards(transform.position,
            patrolPoints[currentPointIndex].position, speed*Time.deltaTime);

        
        if (transform.position == patrolPoints[currentPointIndex].position)
        {
            transform.rotation = patrolPoints[currentPointIndex].rotation;
            anim.SetBool("isRunning", false);
            if (waitTime <= 0)
            {
                if (currentPointIndex + 1 < patrolPoints.Length)
                {
                    currentPointIndex++;
                }
                else currentPointIndex = 0;

                waitTime = startWaitTIme;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        } else anim.SetBool("isRunning", true) ;
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.tag == "Player")
    //     {
    //         collision.GetComponent<Player>().TakeDamage(damage);
    //     }
    // }
}

