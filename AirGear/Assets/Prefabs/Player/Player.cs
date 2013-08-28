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
	
	public int maxSpeed, jumpSpeed, maxGravity;
	public float gravityRate;
	float currentSpeed, ySpeed;
	Vector3 moveDirection;
	CharacterController cc;
	
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
	}
	
	void PlayerInput()
	{
		if(Input.GetButtonDown("Jump"))
			if(cc.isGrounded) Jump ();
	}
	
	void Movement()
	{
		if(!cc.isGrounded)
		{
			if(ySpeed > maxGravity)
				ySpeed += gravityRate*Time.deltaTime;
			else
				ySpeed = maxGravity;
		}
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), ySpeed, (Input.GetAxis("Vertical")));
		cc.Move (moveDirection*maxSpeed*Time.deltaTime);
	}
	
	void Jump()
	{
		ySpeed = jumpSpeed*Time.deltaTime;
	}
}
