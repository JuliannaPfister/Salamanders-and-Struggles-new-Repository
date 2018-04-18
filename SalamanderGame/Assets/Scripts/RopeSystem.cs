using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSystem : MonoBehaviour 
{
	//keeps track of the different components that the RopeSystem script will interact with
	public GameObject ropeHingeAnchor;

	public DistanceJoint2D ropeJoint;

	public Transform crosshair;

	public SpriteRenderer crosshairSprite;

	public PlayerMovement playerMovement;

	private bool ropeAttached;

	//public bool isSwinging; 

	private Vector2 playerPosition;

	private Rigidbody2D ropeHingeAnchorRb;

	private SpriteRenderer ropeHingeAnchorSprite;

	//holds a regerence to the line renderer that will display the rope
	public LineRenderer ropeRenderer;

	//allows to customize which physics layers the "grapple hook"'s raycast will be able to interact with
	public LayerMask ropeLayerMask;

	//sets a maximum distance the raycast can fire.
	private float ropeMaxCastDistance = 20f;

	//tracks the rope wrapping points
	private List<Vector2> ropePositions = new List<Vector2>();

	//private bool isSwinging = false;


	void Awake () 
	{
		//disables the ropeJoint and sets the playerPosition to the current position of the player
		ropeJoint.enabled = false;
		playerPosition = transform.position;
		ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
		ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//captures the world position of the mouse cursor
		var worldMousePosition =
			Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
		//then calculates the facing direction by subtracting the player's position from the mouse position in the world.
		var facingDirection = worldMousePosition - transform.position;
		//using the above, creates the aimAngle
		var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
		if (aimAngle < 0f)
		{
			aimAngle = Mathf.PI * 2 + aimAngle;
		}

		// rotation for later use
		var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
		//player position is tracked
		playerPosition = transform.position;

		//uses to determine if the rope is attached to an anchor point.
		if (!ropeAttached)
		{
			SetCrosshairPosition (aimAngle);
		}
		else
		{
			crosshairSprite.enabled = false;
		}

		HandleInput(aimDirection);

	}

	//positions the crosshair based on the aimAngle that is passed in.
	private void SetCrosshairPosition(float aimAngle)
	{
		if (!crosshairSprite.enabled)
		{
			crosshairSprite.enabled = true;
		}

		var x = transform.position.x + 1f * Mathf.Cos(aimAngle);
		var y = transform.position.y + 1f * Mathf.Sin(aimAngle);

		var crossHairPosition = new Vector3(x, y, 0);
		crosshair.transform.position = crossHairPosition;
	}

	//HandleInput is called from the Update() loop, and polls for input from the left and right mouse buttons
	private void HandleInput(Vector2 aimDirection)
	{
		if (Input.GetMouseButton(0))
		{
			//when it registers a left mouse click, the line rope renderer is enabled and a 2D raycast is fired out from the player's position
			if (ropeAttached) return;
			ropeRenderer.enabled = true;

			var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

			// If a valid raycast hit is found, ropeAttached is set to true, and a check is done on the list of rope vertex positions to make sure the point hit isn't in there already.
			if (hit.collider != null)
			{
				ropeAttached = true;
				if (!ropePositions.Contains(hit.point))
				{
					// Provided the above check is true, then a small impulse force is added to the slug to hop him up off the ground, and the ropeJoint (DistanceJoint2D) is enabled, and set with a distance equal to the distance between the player and the raycast hitpoint. The anchor sprite is also enabled.
					// Jump slightly to distance the player a little from the ground after grappling to something.
					transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
					ropePositions.Add(hit.point);
					ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
					ropeJoint.enabled = true;
					ropeHingeAnchorSprite.enabled = true;
				}
			}
			// If the raycast doesn't hit anything, then the rope line renderer and rope joint are disabled, and the ropeAttached flag is set to false.
			else
			{
				ropeRenderer.enabled = false;
				ropeAttached = false;
				ropeJoint.enabled = false;
			}
		}

		if (Input.GetMouseButton(1))
		{
			ResetRope();
		}
	}

	// If the right mouse button is clicked, the ResetRope() method is called, which will disable and reset all rope/grappling hook related parameters to what they should be when the grappling hook is not being used.
	private void ResetRope()
	{
		ropeJoint.enabled = false;
		ropeAttached = false;
		playerMovement.isSwinging = false;
		ropeRenderer.positionCount = 2;
		ropeRenderer.SetPosition(0, transform.position);
		ropeRenderer.SetPosition(1, transform.position);
		ropePositions.Clear();
		ropeHingeAnchorSprite.enabled = false;
	}
}
