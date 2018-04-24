using UnityEngine;

public class PlayerMovementALTERNATE : MonoBehaviour
{

	//FOR SWINGING
    public float swingForce = 4f;
    public Vector2 ropeHook;
    public bool isSwinging;

	//FOR MOVING
	public float speed = 1f;
	public float topSpeed = 1f;
	public float sprintSpeed = 5f;
	public float jumpSpeed = 3f;
	private float jumpInput;
	private float horizontalInput;

	public bool groundCheck;
	public bool sprinting;


	//player health
	public int health = 5;

	//MISC
	private SpriteRenderer playerSprite;
	private Rigidbody2D rBody;
	private bool isJumping;
	private Animator animator;

    void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        jumpInput = Input.GetAxis("Jump");
		float horizontalInput = Input.GetAxis("Horizontal");
		//adds velocity to the rigidbody in the move direction * speed
		GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalInput * topSpeed, GetComponent<Rigidbody2D>().velocity.y);

        var halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        groundCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.04f), Vector2.down, 0.025f);
    }

    void FixedUpdate()
    {
        if (horizontalInput < 0f || horizontalInput > 0f)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            playerSprite.flipX = horizontalInput < 0f;
            if (isSwinging)
            {
                animator.SetBool("IsSwinging", true);

                // Get normalized direction vector from player to the hook point
                var playerToHookDirection = (ropeHook - (Vector2) transform.position).normalized;

                // Inverse the direction to get a perpendicular direction
                Vector2 perpendicularDirection;
                if (horizontalInput < 0)
                {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                    var leftPerpPos = (Vector2) transform.position - perpendicularDirection*-2f;
                    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                }
                else
                {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                    var rightPerpPos = (Vector2) transform.position + perpendicularDirection*2f;
                    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                }

                var force = perpendicularDirection * swingForce;
                rBody.AddForce(force, ForceMode2D.Force);
            }
            else
            {
                animator.SetBool("IsSwinging", false);
                if (groundCheck)
                {
                    var groundForce = speed*2f;
                    rBody.AddForce(new Vector2((horizontalInput*groundForce - rBody.velocity.x)*groundForce, 0));
                    rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
                }
            }
        }
        else
        {
            animator.SetBool("IsSwinging", false);
            animator.SetFloat("Speed", 0f);
        }

        if (!isSwinging)
        {
            if (!groundCheck) return;

            isJumping = jumpInput > 0f;
            if (isJumping)
            {
                rBody.velocity = new Vector2(rBody.velocity.x, jumpSpeed);
            }
        }

		if (Input.GetKey(KeyCode.LeftShift))
		{
			sprinting = true;
			topSpeed = sprintSpeed;
		}

		else

		{
			sprinting = false;
			topSpeed = speed;
		}



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

	public void OnCollisionEnter2D (Collision2D collision)
	{

		if (collision.gameObject.layer == LayerMask.NameToLayer("HealthPickUp"))
		{
			collision.gameObject.SetActive(false);
			health++;
			// collectSound.Play();

		}
			

	}
}
