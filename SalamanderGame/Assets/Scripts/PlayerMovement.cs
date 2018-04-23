using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PlayerMovement : MonoBehaviour
{

    //MOVEMENT 

    // the player speed
    public float topSpeed = 5f;
    public float sprintSpeed = 10f;
    // always set this to be equal to rhe topSpeed in the inspector
    public float normalSpeed = 5f;
    //tells the direction the player is facing
    bool facingRight = true;
    
    //get animator
    Animator anim;
    //not grounded
    bool grounded = false;
    public Transform groundChecker;
    float groundRadius = 0.2f;
    //the jump force
    public float jumpForce = 700f;
    public float climbingJumpForce = 700f;
    // what layer is considered ground
    public LayerMask whatIsGround;
    //the rigidbody
    private Rigidbody2D rb;
    public bool sprinting;

	//SWINGING
	public bool isSwinging;
	public bool groundCheck;
	public float swingForce = 4f;
	public Vector2 ropeHook;
	private bool isJumping;
	private float horizontalInput;
	private float jumpInput;
	//public bool groundCheck;



    //COMBAT

    //player health
    public int health = 5;
    // invincible time after player gets hurt
    //public float blinkTime = 2f;
    //fire point for player attack 1
    public Transform firePoint;
    //assign the weapon for attack 1 
    public GameObject adaga;

    //Particle effects
    public GameObject ps;

    //UI score
    public int score;
    public Text scoreText;

    //soundeffects
    //public AudioSource jumpSound;
    //public AudioSource collectSound;
    //public AudioSource adagaSound;


	//=======WallJump======================

	//public float leftbounceAmount = 200f;
	//public float bounceAmount = 200f;




    
    void Awake()
    {


        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        

    }





    void FixedUpdate()
    {
        
        // Add force down to make the jump feel nicer 
        rb.AddForce(Vector2.up * -50f);

        

            // if the ground transform hit the whatisground with groundradius
            grounded = Physics2D.OverlapCircle(groundChecker.position, groundRadius, whatIsGround);

        //tells the animator that player is grounded
        //anim.SetBool("Ground", grounded);


        //move direction
        float move = Input.GetAxis("Horizontal");

        //adds velocity to the rigidbody in the move direction * speed
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * topSpeed, GetComponent<Rigidbody2D>().velocity.y);
        // gets how fast we move up or down from the rigidbody
       // anim.SetFloat("Speed", Mathf.Abs(move));

        // if player is moving left calls the function Flip() if it moves right call function Flip() again
        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();


        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprinting = true;
            topSpeed = sprintSpeed;
        }

        else

        {
            sprinting = false;
            topSpeed = normalSpeed;
        }


    }

    void Update()
    {
        //if player is on the ground and space key is pressed
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {   //Set isgrounded to false
            //anim.SetBool("Ground", false);
            //and add jump force
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            //jumpSound.Play();
        }

        if (grounded && Input.GetKeyDown(KeyCode.W))
        {   //Set isgrounded to false
            //anim.SetBool("Ground", false);
            //and add jump force
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            //jumpSound.Play();
        }

		//groundCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.04f), Vector2.down, 0.025f);

      
        

        //attack 1 button
        //if left mouse botton is pressed
        //if (Input.GetMouseButtonDown(0))
        //instantiate the game object adaga on the fire points' position and rotation
        //{
           // Instantiate(adaga, firePoint.position, firePoint.rotation);
            //adagaSound.Play();
       // }


      

       
    }




    //the function Flip transforms the scale to flip the player object left or right
    void Flip()
    {
        facingRight = !facingRight;
        //get local scale
        Vector3 theScale = transform.localScale;
        //flips on x axis
        theScale.x *= -1;
        //appy to local scale
        transform.localScale = theScale;
    }

     

  

    public void Hurt()
    {
        // if health is lower than zero, restart game
        health--;
        if (health <= 0)
            Application.LoadLevel(Application.loadedLevel);

        //if health isnt lower than zero, triggers the blink time
        //else
            //TriggerHurt(blinkTime);
    }

   // public void TriggerHurt(float hurtTime)
    //{
       // StartCoroutine(HurtBlinker(hurtTime));
   // }

    //IEnumerator HurtBlinker(float hurtTime)
    //{
        //ignore collision with enemies
        //int enemyLayer = LayerMask.NameToLayer("Enemy");
        //int playerLayer = LayerMask.NameToLayer("Player");


        //ignores colision (damage) 
        //Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer);
        //start looping blinking anim
        //anim.SetLayerWeight(1, 1);

        //wait for invincibility to end
        //yield return new WaitForSeconds(hurtTime);
        //stops blinking animation and re enable collision (damage)
       // Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer, false);
        //anim.SetLayerWeight(1, 0);
    //}




    public void OnCollisionEnter2D(Collision2D collision)
    {
        //if player collides with enemy, calls function Hurt()
        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)


        {
            Hurt();
        }

        // if player collides with gameobject with layer mask "traps" calls function Hurt()
        if (collision.gameObject.layer == LayerMask.NameToLayer("Traps"))
        {
            Hurt();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyAdaga"))
        {
            Hurt();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Hurt();
        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("GameOver"))
        {
            Hurt();
            Application.LoadLevel(Application.loadedLevel);
        }

        if (collision.transform.tag == "Platform")
        {
            transform.parent = collision.transform;

        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("PickUp"))
        {
            collision.gameObject.SetActive(false);
            score++;
            scoreText.text = score.ToString();
           // collectSound.Play();



        }

       

        if (collision.gameObject.layer == LayerMask.NameToLayer("HealthPickUp"))
        {
            collision.gameObject.SetActive(false);
            health++;
           // collectSound.Play();


        }


       





        //========================WallJump====================================


            //if (collision.gameObject.tag == "WallRight" && (Input.GetKeyDown ("space"))) {
            //	rb.AddForce (Vector3.up * bounceAmount);
            //	rb.AddForce (Vector3.left * leftbounceAmount);

            //}

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "ClimbingWalls" && Input.GetKey(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0,  climbingJumpForce));
        }

    }

    void OnCollisionExit2D(Collision2D collision)
    {


        if (collision.transform.tag == "platform")
        {
            transform.parent = null;

        }
    }

}
