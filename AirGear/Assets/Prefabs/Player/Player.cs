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
	CharacterController cc, camController;
	
	#region CameraVars
	public int rotateSpeed;
	public float minCamX, maxCamX, cameraRotationX;
	public Vector3 cameraOffset; //Position relative to player
	public GameObject cam;
	#endregion

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
		camController = cam.GetComponent<CharacterController>();
		currentSpeed = maxSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		PlayerInput();
		Movement();
		CameraControl();
		//CameraMinMax();
	}
	
	void PlayerInput()
	{
		if(Input.GetButtonDown("Jump"))
			if(cc.isGrounded) Jump ();

	}
	
	void OnGUI()
	{
		if(!Screen.lockCursor)
		{
			if(GUI.Button(new Rect(0,0,Screen.width/12,Screen.width/25),"Lock Cursor"))
				Screen.lockCursor = true;
		}
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
	
	void CameraControl()//Controls camera view
	{
//		cam.transform.LookAt(transform);
//		if(cam.transform.localPosition.x > minCamX && cam.transform.localPosition.x < maxCamX)
//			camController.Move(Vector3.right*Input.GetAxis("Mouse X")*rotateSpeed*Time.deltaTime);
//		else
//		{
//			if(cam.transform.localPosition.x < minCamX)
//				cam.transform.localPosition = new Vector3(minCamX,cam.transform.localPosition.y,cam.transform.localPosition.z);
//			if(cam.transform.localPosition.x > maxCamX)
//				cam.transform.localPosition = new Vector3(maxCamX,cam.transform.localPosition.y,cam.transform.localPosition.z);
//		}
		//cam.transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);
	}

//	void CameraMinMax()//Correct vertical camera to stay within bounds
//	{
//		float zRot;
//		if(Input.GetAxis("Mouse Y")>.5)
//			zRot=.5f;
//		else if(Input.GetAxis("Mouse Y")<-.5)
//			zRot=-.5f;
//		else
//			zRot = Input.GetAxis("Mouse Y");
//
//		if(cameraRotationX >= minCamX && cameraRotationX <= maxCamX)
//		{
//			cameraRotationX += zRot;
//			cam.transform.Rotate(new Vector3(Time.deltaTime*zRot*-rotateSpeed,0,0));
//		}
//		if(cameraRotationX < minCamX)//if lower than min rotation, correct
//		{
//			cam.transform.Rotate(new Vector3(Time.deltaTime*.6f*-rotateSpeed,0,0));
//			cameraRotationX += Time.deltaTime*-.3f*-rotateSpeed;
//		}
//		if(cameraRotationX > maxCamX)//if higher than max rotation, correct
//		{
//			cameraRotationX += Time.deltaTime*.3f*-rotateSpeed;
//			cam.transform.Rotate(new Vector3(Time.deltaTime*-.6f*-rotateSpeed,0,0));
//		}	
//	}
}

