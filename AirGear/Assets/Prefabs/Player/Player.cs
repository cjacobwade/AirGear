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
	
	public int maxSpeed, jumpSpeed, wallSpeed, maxGravity, hookDistance;
	public float xSpeed, zSpeed, gravityRate, charWidth, speedRate, decayRate;
	public Transform rotationCube, cam;
	PlayerCam _cam;
	bool wall;
	public float currentSpeed, ySpeed;
	public Vector3 moveDirection, hookOffset, hitPosition;
	public RaycastHit wallHit;
	public LayerMask wallLayer;
	CharacterController cc;

	public enum playerState
	{
		idle,
		moving,
		jumping,
		wallriding,
		grinding,
		//hooking
	}
	
	public playerState currentState;
	
	// Use this for initialization
	void Awake () 
	{
		_cam = cam.GetComponent<PlayerCam>();
		cc = GetComponent<CharacterController>();
		currentSpeed = 7;
	}
	
	// Update is called once per frame
	void Update () 
	{
		PlayerInput();
		PlayerState();
		SpeedControl();
		Movement();
		//Hookshot();
	}
	
	void LateUpdate()
	{
		rotationCube.transform.position = transform.position;
		transform.rotation = Quaternion.Lerp(transform.rotation,rotationCube.transform.rotation,0.7f);
	}
	
//	void Hookshot()
//	{
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		if(Input.GetMouseButtonDown(0))
//		{
//			if(Physics.Raycast(ray.origin,ray.direction*50,out wallHit, hookDistance, wallLayer))
//			{
//				hitPosition = wallHit.point;
//				print ("Hit");
//			}
//		}
//		if(Input.GetMouseButton(0))
//		{
//			Debug.DrawLine(_cam.camera.ScreenPointToRay(Input.mousePosition).origin,_cam.transform.right,Color.red);
//			Debug.DrawRay(ray.origin,ray.direction*50,Color.green);
//			if(wallHit.transform != null && Vector3.Distance(transform.position,hitPosition) > hookDistance)
//			{
//				currentState = playerState.hooking;
//				print ("Hooking");
//				moveDirection = wallHit.point - transform.position;
//			}
//			else
//			{
//				moveDirection = new Vector3(xSpeed*currentSpeed, ySpeed*maxSpeed,zSpeed*currentSpeed);
//				moveDirection = transform.TransformDirection(moveDirection);
//			}
//		}
//		else
//		{
//			moveDirection = new Vector3(xSpeed*currentSpeed, ySpeed*maxSpeed,zSpeed*currentSpeed);
//			moveDirection = transform.TransformDirection(moveDirection);
//			//currentState = playerState.idle;	
//		}
//
//	}
	
	void PlayerInput()
	{
		if(Input.GetButtonDown("Jump"))
			if(cc.isGrounded) Jump ();
	}
	
	void PlayerState()
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position,transform.right,out hit,charWidth/1.2f))
		{
			WallRide();
			if(moveDirection.x == 0 && moveDirection.z == 0) currentSpeed = 7;
			else
			{
				if(cc.isGrounded||(currentState != playerState.jumping && currentState != playerState.grinding))
				{
					//if(currentState != playerState.hooking)
					currentState = playerState.moving;
					wall = false;
				}
			}
		}
		if(moveDirection == Vector3.zero /*&& currentState != playerState.hooking*/) currentState = playerState.idle;
	}
	
	void Movement()
	{
		if(cc.isGrounded /*|| currentState == playerState.hooking*/)
		{
			maxGravity = -1;
		}
		else
			maxGravity = -9;
		if(ySpeed > maxGravity)
			ySpeed += gravityRate*Time.deltaTime;
		else ySpeed = maxGravity;
		xSpeed = Input.GetAxis("Horizontal");
		zSpeed = Input.GetAxis("Vertical");
		moveDirection = new Vector3(xSpeed*currentSpeed, ySpeed*maxSpeed,zSpeed*currentSpeed);
		moveDirection = transform.TransformDirection(moveDirection);
		cc.Move (moveDirection*Time.deltaTime);
	}
	
	void WallRide(RaycastHit hit, string direction)
	{
		/*if(currentState != playerState.hooking)*/ currentState = playerState.wallriding;
		if(currentSpeed > wallSpeed) currentSpeed--;
		if(Input.GetButtonDown("Jump")) WallJump();
		
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
			if(currentSpeed < maxSpeed*0.5f) currentSpeed += speedRate;
		if(Mathf.Abs(xSpeed) > 0.01f) 
			if(currentSpeed < maxSpeed*0.5f) currentSpeed += speedRate;
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

