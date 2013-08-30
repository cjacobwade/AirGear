using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	
	public Transform player;
	private Player _player;
	public Vector2 rotateSpeed, verticalLimit;
	Vector2 inputVector;
	Vector3 offset;
	Quaternion camRotation;
	
	// Use this for initialization
	void Awake () 
	{
		_player = player.gameObject.GetComponent<Player>();
		offset = player.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		CameraControl();
	}
	
	float ClampAngle(float angle, float min, float max)
	{
		if(angle < -360)
			angle +=360;
		if(angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle,min,max);
	}
	
	void OnGUI()
	{
		if(!Screen.lockCursor)
		{
			if(GUI.Button(new Rect(0,0,Screen.width/12,Screen.width/25),"Lock Cursor"))
				Screen.lockCursor = true;
		}
		
		GUI.Label(new Rect(Screen.width-100,0,500,25),_player.moveDirection.ToString());
		GUI.Label(new Rect(Screen.width-100,25,500,25),_player.ySpeed.ToString());
		GUI.Label(new Rect(Screen.width-100,50,500,25),_player.currentSpeed.ToString());
		GUI.Label(new Rect(Screen.width-100,75,500,25),_player.currentState.ToString());
		rotateSpeed.x = GUI.VerticalSlider(new Rect(Screen.width*9.8f/10,100,10,400),rotateSpeed.x,30,200);
		rotateSpeed.y = GUI.VerticalSlider(new Rect(Screen.width*9.7f/10,100,10,400),rotateSpeed.y,30,200);
	}
	
	void CameraControl()
	{
		if(Mathf.Abs(Input.GetAxis("Camera X")) > 0.01)
		inputVector.x += Input.GetAxis("Camera X")*rotateSpeed.x*Time.deltaTime;
		if(Mathf.Abs(Input.GetAxis("Camera Y")) > 0.01)
		inputVector.y += Input.GetAxis("Camera Y")*rotateSpeed.y*Time.deltaTime;
		
		//Keep vertical rotation within bounds specified in the inspector
		inputVector.y = ClampAngle(inputVector.y,verticalLimit.x,verticalLimit.y);
		
		//Set rotation variable to match the current input
		Quaternion rotation = Quaternion.Euler(inputVector.y,inputVector.x,0);
		
		//Set the position relative to the player and based on rotation
		Vector3 position = player.position - rotation*offset;
	
		//Actually perform the changes
		transform.rotation = rotation;
		transform.position = position;
		
		//Rotate the player so they're facing where the camera is facing
		if(_player.currentState != Player.playerState.wallriding)
			player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.Euler(0,inputVector.x,0),0.5f);//should only do this if the player instances speed is greater than 0
	}
}
