using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
   public float speed, lifeTime;
   public int damage;

   private void Start()
   {
      Destroy(gameObject, lifeTime);
   }

   private void Update()
   {
      transform.Translate(Vector2.left * speed * Time.deltaTime);
   }
   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.tag == "Player")
      {
         collision.GetComponent<Player>().TakeDamage(damage);
      }
      Destroy(gameObject);
   }
}
