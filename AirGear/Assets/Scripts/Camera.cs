using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	
	public Transform player;
	public Vector2 rotateSpeed, verticalLimit;
	Vector2 inputVector;
	Vector3 offset;
	Quaternion camRotation;
	
	// Use this for initialization
	void Awake () 
	{
		offset = player.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () 
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
		player.transform.rotation = Quaternion.Euler(0,inputVector.x,0);//should only do this if the player instances speed is greater than 0
		
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
	}
}
