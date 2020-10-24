using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Player : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;
    bool facingRight = true;
   
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpForce;

    private bool isTouchingFront;
    public Transform frontCheck;
    private bool wallSliding;
    public float wallSlidingSpeed;

    private bool wallJumpping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;

    Animator anim;

    public int health;

    public float timeBeetwenAttacks;
    private float nextAttackTime;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;

    public int damage;

    public SpriteRenderer weaponRenderer;

    public GameObject blood;
    public GameObject stars;

    private AudioSource source;
    
    public AudioClip jumpSound;
    public AudioClip hurtSound;
    public AudioClip pickupSound;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        

        if (Time.time > nextAttackTime)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                FindObjectOfType<CameraShake>().Shake();
                anim.SetTrigger("attack");
                nextAttackTime = Time.time + timeBeetwenAttacks;
            }
        }
        
        float input = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(input * speed, rb.velocity.y );
        
        
        if (input > 0 && facingRight == false)
        {
            Flip();
        } else if (input < 0 && facingRight == true) {
            Flip();
        }
        
        
        if (input != 0)
        {
            anim.SetBool("IsRunning",true);
        }
        else if(input == 0)
        {
            anim.SetBool("IsRunning",false);
        }
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);
        
        
        
        
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
            source.clip = jumpSound;
            source.Play();
        }
        
        if (isTouchingFront == true && isGrounded == false && input != 0)
        {
            wallSliding = true;
        }
        else wallSliding = false;

        if (isGrounded == true)
        {
            anim.SetBool("IsJumping", false);    
        } else  
            anim.SetBool("IsJumping", true);    
        
        if (wallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x,
                Mathf.Clamp(rb.velocity.y,-wallSlidingSpeed,float.MaxValue));
            source.clip = jumpSound;
            source.Play();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && wallSliding == true)
        {
            wallJumpping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (wallJumpping == true)
        {
            rb.velocity = new Vector2(xWallForce * -input, yWallForce);
        }
    }

    void Flip()
    {
       transform.localScale = new Vector3(-transform.localScale.x,
       transform.localScale.y, transform.localScale.z);
       facingRight = !facingRight;
    }
 
    void SetWallJumpingToFalse()
    {
        wallJumpping = false;
        
    }

    public void TakeDamage(int damage)
    {
        source.clip = hurtSound;
        source.Play();
        FindObjectOfType<CameraShake>().Shake();
        health -= damage;
        print(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        Instantiate(blood, transform.position, Quaternion.identity);
    }

    public void Attack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D col in enemiesToDamage)
        {
            col.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void Equip(Weapon weapon)
    {
        source.clip = pickupSound;
        source.Play();
        damage = weapon.damage;
        attackRange = weapon.attackRange;
        weaponRenderer.sprite = weapon.GFX;
        Destroy(weapon.gameObject);
        Instantiate(stars, transform.position, Quaternion.identity);
    }
}
