using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D), typeof(Animator))]

public class SoyBoyController : MonoBehaviour {

	// using for running when press arrow button
	public float speed = 14f;
	public float accel = 6f;
	private Vector2 input; // the controller's current input values
	private SpriteRenderer sr;
	private Rigidbody2D rb;
	private Animator animator;

	// for jumping
	public bool isJumping;
	public float jumpSpeed = 8f;
	private float rayCastLengthCheck = 0.005f;
	private float width;
	private float height;

	public float jumpDurationThreshold = 0.25f;
	private float jumpDuration;

	// used for air control, which is the acceleration in air
	public float airAccel = 3f;

	public float jump = 14f;

	// for sound effects
	public AudioClip runClip;
	public AudioClip jumpClip;
	public AudioClip slideClip;
	private AudioSource audioSource;



	void Awake(){
		sr = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();

		//The extents of the box. This is always half of the size.
		width = GetComponent<Collider2D> ().bounds.extents.x + 0.1f; 
		height = GetComponent<Collider2D> ().bounds.extents.y + 0.2f;

		// cache a reference to the AudioSource component 
		audioSource = GetComponent<AudioSource>();

	}

	// Use this for initialization
	void Start () {
		
	}


	// check whether the player is on the ground by rayCast
	// public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);

	public bool PlayerIsOnGround(){

		// this is the implicit conversion from RayCastHit2D to bool
		bool groundCheck1 = Physics2D.Raycast (new Vector2(transform.position.x, transform.position.y - height), -Vector2.up, rayCastLengthCheck);
		bool groundCheck2 = Physics2D.Raycast (new Vector2(transform.position.x + width - 0.2f , transform.position.y - height), -Vector2.up, rayCastLengthCheck);
		bool groundCheck3 = Physics2D.Raycast (new Vector2(transform.position.x - width + 0.2f, transform.position.y - height), -Vector2.up, rayCastLengthCheck);

		if (groundCheck1 || groundCheck2 || groundCheck3) {
			return true;
		} else {
			return false;
		}
			
	}

	// check whether the player is about to touch the wall
	public bool IsWallToLeftOrRight(){

		// the idea is same as the PlayerIsOnGround()
		bool wallOnleft = Physics2D.Raycast(new Vector2(
			transform.position.x - width, transform.position.y),
			-Vector2.right, rayCastLengthCheck);
		
		bool wallOnRight = Physics2D.Raycast(new Vector2(
			transform.position.x + width, transform.position.y),
			Vector2.right, rayCastLengthCheck);
		
		if (wallOnleft || wallOnRight) {
			return true;
		}
		else {
			return false;
		}
	}

	// the combine part on ground or wall
	public bool PlayerIsTouchingGroundOrWall(){
		if (PlayerIsOnGround () || IsWallToLeftOrRight ()) {
			return true;
		} else {
			return false;
		}
	}

	public int GetWallDirection(){
		bool isWallLeft = Physics2D.Raycast(new Vector2(
			transform.position.x - width, transform.position.y),
			-Vector2.right, rayCastLengthCheck);
		bool isWallRight = Physics2D.Raycast(new Vector2(
			transform.position.x + width, transform.position.y),
			Vector2.right, rayCastLengthCheck);
		if (isWallLeft) {
			return -1;
		}
		else if (isWallRight) {
			return 1; 
		}
		else {
			return 0; 
		}
	}

	void PlayAudioClip(AudioClip clip){
	
		if (audioSource != null && clip != null) {
			if (!audioSource.isPlaying) {
				audioSource.PlayOneShot (clip);
			}
		}
	
	}
	// Update is called once per frame
	void Update () {
		input.x = Input.GetAxis ("Horizontal");
		input.y = Input.GetAxis ("Jump");

		animator.SetFloat ("Speed", Mathf.Abs(input.x));

		if (input.x > 0f) {
			sr.flipX = false;
		}
		else if(input.x < 0f){
			sr.flipX = true;
		}

		if (input.y > 0f) {
			jumpDuration += Time.deltaTime;
			animator.SetBool ("IsJumping", true);
		} else {
			isJumping = false;
			animator.SetBool ("IsJumping", false);
			jumpDuration = 0f;
		}

		if (PlayerIsOnGround () && !isJumping) {
			if(input.y > 0f){
				isJumping = true;
				PlayAudioClip (jumpClip);
//				animator.SetBool ("IsOnWall", false);
			}
			animator.SetBool ("IsOnWall", false);
			if (input.x < 0f || input.x > 0f) {
				PlayAudioClip (runClip);
			}
		}

		if (jumpDuration > jumpDurationThreshold) {
			input.y = 0f;
		}



	}

	// which is called at fixed frame rate, especially use on the rigid movement
	void FixedUpdate(){
		var acceleration = 0f;
		if (PlayerIsOnGround ()) {
			acceleration = accel;
		} else {
			acceleration = airAccel;
		}

		var xVelocity = 0f;
		if (PlayerIsOnGround() && input.x == 0) {
			xVelocity = 0f;
		} else {
			xVelocity = rb.velocity.x;
		}

		var yVelocity = 0f;
		if (PlayerIsTouchingGroundOrWall () && input.y == 1) {
			yVelocity = jump;
		} else {
			yVelocity = rb.velocity.y;
		}


		rb.AddForce (new Vector2 ( ((input.x * speed) - rb.velocity.x)*acceleration, 0));

		rb.velocity = new Vector2 (xVelocity, yVelocity);

		if (IsWallToLeftOrRight () && !PlayerIsOnGround () && input.y == 1) {
			rb.velocity = new Vector2 (-GetWallDirection () * speed * 0.75f, rb.velocity.y);
			animator.SetBool ("IsOnWall", false);
			animator.SetBool ("IsJumping", true);
		} else if (!IsWallToLeftOrRight ()) { // ???????
			animator.SetBool ("IsOnWall", false);
			animator.SetBool ("IsJumping", true);
		}
			
		if (IsWallToLeftOrRight () && !PlayerIsOnGround ()) {
			animator.SetBool ("IsOnWall", true);
			PlayAudioClip (slideClip);
		}

		if (isJumping && jumpDuration < jumpDurationThreshold) {
			rb.velocity = new Vector2 (rb.velocity.x, jumpSpeed);
		}
	}

}
