using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	#region Mechanics
		//Max Speed
		//Turn Speed
			//Hookshot
		//Tricks
			//Double jump if completing a different trick
			//Wall Riding
			//Grinding
	#endregion
	
	public int maxSpeed, jumpSpeed, wallSpeed, maxGravity;
	public float gravityRate, charWidth;
	public float currentSpeed, ySpeed;
	public Vector3 moveDirection;
	CharacterController cc;

	public enum playerState
	{
		idle,
		moving,
		jumping,
		wallriding,
		grinding,
	}
	
	public playerState currentState;
	
	// Use this for initialization
	void Awake () 
	{
		cc = GetComponent<CharacterController>();
		currentSpeed = maxSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		PlayerInput();
		PlayerState();
		Movement();
	}
	
	void PlayerInput()
	{
		if(Input.GetButtonDown("Jump"))
			if(cc.isGrounded) Jump ();
	}
	
	void PlayerState()
	{
		RaycastHit hit;
		Vector3 center = transform.position, right = transform.right;
		if(Physics.Raycast(center,right,out hit,charWidth/1.3f)||Physics.Raycast(center,-right,out hit,charWidth/1.3f))
			WallRide();
		else
		{
			currentSpeed = maxSpeed;
			if(moveDirection.x == 0 && moveDirection.z == 0) currentState = playerState.idle;
			else
			{
				if(cc.isGrounded||(currentState != playerState.jumping && currentState != playerState.grinding))
					currentState = playerState.moving;
			}
		}
	}
	
	void Movement()
	{
		if(ySpeed > maxGravity)
			ySpeed += gravityRate*Time.deltaTime;
		moveDirection.y = 0;
		moveDirection.Normalize();
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), ySpeed, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		cc.Move (moveDirection*currentSpeed*Time.deltaTime);
	}
	
	void WallRide()
	{
		currentState = playerState.wallriding;
		if(currentSpeed > wallSpeed)
			currentSpeed--;
		if(Input.GetButtonDown("Jump"))
			WallJump();
	}
	
	void Jump()
	{
		ySpeed = jumpSpeed*Time.deltaTime;
	}
	
	void WallJump()
	{
		
		ySpeed = jumpSpeed*Time.deltaTime;
	}

}

