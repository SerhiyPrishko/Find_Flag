﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed;
    int currentPointIndex;

    private float waitTime;
    public float startWaitTIme;
    
    
    private void Start()
    {
        transform.position = patrolPoints[0].position;
        waitTime = startWaitTIme;

    }

    private void Update(){
        transform.position = Vector2.MoveTowards(transform.position,
            patrolPoints[currentPointIndex].position, speed*Time.deltaTime);

        
        if (transform.position == patrolPoints[currentPointIndex].position)
        {
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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.parent = transform;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
}
