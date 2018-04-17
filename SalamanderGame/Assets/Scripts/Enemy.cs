using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //enemy health
    public int currentHealth;
    // the enemy speed
    public float velocity = 1f;
    //rigidbody
    private Rigidbody2D rb;
    //the enemy sight start point
    public Transform sightStart;
    //the enemy sight end point
    public Transform sightEnd;
    // the points when enemy detects playeer
    public Transform playerDetectPointA;
    public Transform playerDetectPointB;
    // the points when enemy detects playeer (back)
    public Transform playerBackDetectPointA;
    public Transform playerBackDetectPointB;
    //determines with enemy collided with walls/tiles 
    public bool colliding;
    // layer mask to define what the enemy will perceve as a colision
    public LayerMask detectWhat;
    public LayerMask detectPlayer;
    //get animator
    Animator anim;
    public GameObject eneWeapon;

    public Transform firePoint;

    private PlayerMovement plyrMov;

    public bool seePlayer;
    public bool seePlayerBack;

    public float waitBetweenShots;
    private float shotCounter;


    // Use this for initialization
    void Start()
    {
        // get the component rigidbody2d
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        plyrMov = FindObjectOfType<PlayerMovement>();
        shotCounter = waitBetweenShots;

    }

    // Update is called once per frame
    void Update()
    {
        shotCounter -= Time.deltaTime;
        if (seePlayer && shotCounter<0)
        //instantiate the game object adaga on the fire points' position and rotation
        {
            Instantiate(eneWeapon, firePoint.position, firePoint.rotation);
            plyrMov.Hurt();
            shotCounter = waitBetweenShots;
        }


        //direction the enemy moves to
        rb.velocity = new Vector2(velocity, rb.velocity.y);
        //cast a horrizontal line to detect colision with walls
        colliding = Physics2D.Linecast(sightStart.position, sightEnd.position, detectWhat);
        //cast a horrizontal line to detect Player
        seePlayer = Physics2D.Linecast(playerDetectPointA.position, playerDetectPointB.position, detectPlayer);
        seePlayerBack = Physics2D.Linecast(playerBackDetectPointA.position, playerBackDetectPointB.position, detectPlayer);

        //if the enemy detects a colision, transform the local scale and change moving direction
        if (colliding)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            velocity *= -1;
        }


        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        if (seePlayerBack)
       
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            velocity *= -1;


        }
    }

 



    public void OnTriggerEnter2D(Collider2D other)
    {
        //if enemy collides with weapon 
        if (other.tag == "Weapon")
        {   
            //loses -1 health
            currentHealth--;
            //blinks when takes damage
            anim.SetLayerWeight(1, 1);
        }

        //need to make it stop blinking after 2 secs

   



    }


    

}
