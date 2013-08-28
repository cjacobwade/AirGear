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
	public Vector2 rotateSpeed, camHeight, cameraRot;
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
		cam.transform.position = transform.position + cameraOffset;
		cameraRot.y += Input.GetAxis("Mouse X")*rotateSpeed.x;//Horizontal control
		cameraRot.x += -Input.GetAxis("Mouse Y")*rotateSpeed.y;//Vertical control

		//Keep rotations within -360 and 360
		if(cameraRot.y > 360) cameraRot.y -= 360;
		if(cameraRot.y < -360) cameraRot.y += 360;

		//Regulate camera height
		if (cameraRot.x < camHeight.x) cameraRot.x = camHeight.x;
		if (cameraRot.x > camHeight.y) cameraRot.x = camHeight.y;

		//Set camera rotation
		//cam.transform.eulerAngles = cameraRot;
		cam.transform.Rotate(new Vector3(cameraRot.x, cameraRot.y,0)*Time.deltaTime, Space.World);
		//transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraRot.y ,transform.eulerAngles.z);
	}

}

