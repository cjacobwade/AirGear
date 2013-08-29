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
//		player.transform.Rotate(player.transform.eulerAngles.x,Input.GetAxis("Mouse X")*rotateSpeed.x,0);
//		transform.RotateAround(player.transform.position,new Vector3(Input.GetAxis("Mouse Y")*rotateSpeed.y,0,0),rotateSpeed.y*Time.deltaTime);
//		camRotation = Quaternion.Euler(0,player.transform.eulerAngles.y,0);
//		transform.position = player.transform.position - (camRotation*offset);
//		
//		transform.LookAt(player.transform);
		if(Mathf.Abs(Input.GetAxis("Camera X")) > 0.01)
		inputVector.x += Input.GetAxis("Camera X")*rotateSpeed.x*Time.deltaTime;
		if(Mathf.Abs(Input.GetAxis("Camera Y")) > 0.01)
		inputVector.y += Input.GetAxis("Camera Y")*rotateSpeed.y*Time.deltaTime;
		
		inputVector.y = ClampAngle(inputVector.y,verticalLimit.x,verticalLimit.y);
		Quaternion rotation = Quaternion.Euler(inputVector.y,inputVector.x,0);
		Vector3 position = player.position - rotation*offset;
		
		transform.rotation = rotation;
		player.transform.rotation = Quaternion.Euler(0,inputVector.x,0);
		transform.position = position;
		
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
