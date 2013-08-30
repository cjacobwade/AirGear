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
	public float xSpeed, zSpeed, gravityRate, charWidth, speedRate, decayRate;
	public GameObject rotationCube;
	bool wall;
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
		currentSpeed = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		PlayerInput();
		PlayerState();
		SpeedControl();
		Movement();
	}
	
	void LateUpdate()
	{
		rotationCube.transform.position = transform.position;
		transform.rotation = Quaternion.Lerp(transform.rotation,rotationCube.transform.rotation,0.7f);
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
		if(Physics.Raycast(center,right,out hit,charWidth/1.2f)) WallRide(hit, "Right");
		else if(Physics.Raycast(center,-right,out hit,charWidth/1.2f)) WallRide(hit, "Left");
		else
		{
			if(moveDirection.x == 0 && moveDirection.z == 0)
			{
				currentState = playerState.idle;
				currentSpeed = 0;
			}
			else
			{
				if(cc.isGrounded||(currentState != playerState.jumping && currentState != playerState.grinding))
				{
					currentState = playerState.moving;
					wall = false;
				}
			}
		}
	}
	
	void Movement()
	{
		if(cc.isGrounded)
			maxGravity = -1;
		else
			maxGravity = -9;
		if(ySpeed > maxGravity)
			ySpeed += gravityRate*Time.deltaTime;
		else ySpeed = maxGravity;
		moveDirection.y = 0;
		moveDirection.Normalize();
		xSpeed = Input.GetAxis("Horizontal");
		zSpeed = Input.GetAxis("Vertical");
		moveDirection = new Vector3(xSpeed*currentSpeed, ySpeed*maxSpeed,zSpeed*currentSpeed);
		moveDirection = transform.TransformDirection(moveDirection);
		cc.Move (moveDirection*Time.deltaTime);
	}
	
	void WallRide(RaycastHit hit, string direction)
	{
		currentState = playerState.wallriding;
		if(currentSpeed > wallSpeed)
			currentSpeed--;
		if(Input.GetButtonDown("Jump"))
			WallJump();
		
		//This code is borrowed from UnityAnswers and I don't understand it :(
		Vector3 axis;
		axis = (direction == "Right") ? Vector3.Cross(transform.right,-hit.normal) : Vector3.Cross(-transform.right,-hit.normal);
	    if(axis != Vector3.zero)
	    {
			float angle = (direction == "Right") ? Mathf.Atan2(Vector3.Magnitude(axis), Vector3.Dot(transform.right,-hit.normal)): Mathf.Atan2(Vector3.Magnitude(axis), Vector3.Dot(-transform.right,-hit.normal));
		 	//Vector3 meow = transform.RotateAround(axis,angle);
			if(!cc.isGrounded)
				rotationCube.transform.Rotate(axis,angle,Space.World);
        }
	}
	
	void SpeedControl()
	{
		if(zSpeed > 0.01f) 
			if(currentSpeed < maxSpeed) currentSpeed += speedRate;
		if(zSpeed < -0.01f) 
			if(currentSpeed < maxSpeed*0.4f) currentSpeed += speedRate*0.4f;
		if(Mathf.Abs(xSpeed) > 0.01f) 
			if(currentSpeed < maxSpeed*0.4f) currentSpeed += speedRate*0.4f;
		if(currentSpeed > 7) currentSpeed -= decayRate;
		if(currentSpeed < 7) currentSpeed = 7;
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

