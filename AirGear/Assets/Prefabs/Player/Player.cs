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
	float currentSpeed, ySpeed;
	Vector3 moveDirection;
	CharacterController cc;

	public enum playerState
	{
		idle,
		moving,
		jumping,
		wallriding,
		grinding,
	}
	
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
		Movement();
		WallCollision();
	}
	
	void PlayerInput()
	{
		if(Input.GetButtonDown("Jump"))
			if(cc.isGrounded) Jump ();
	}
	
	void Movement()
	{
		Gravity();
		moveDirection.y = 0;
		moveDirection.Normalize();
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), ySpeed, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		cc.Move (moveDirection*currentSpeed*Time.deltaTime);
	}
	
	void Gravity()
	{
		if(!cc.isGrounded)
		{
			if(ySpeed > maxGravity) ySpeed += gravityRate*Time.deltaTime;
			else ySpeed = maxGravity;
		}	
		else
			currentSpeed = maxSpeed;
	}
	
	void WallCollision()
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position,Vector3.right,out hit,charWidth/2))
		{
			if(currentSpeed > wallSpeed)
				currentSpeed--;
			if(Input.GetButtonDown("Jump"))
				Jump ();
		}
	}
	
	void Jump()
	{
		ySpeed = jumpSpeed*Time.deltaTime;
	}

}

