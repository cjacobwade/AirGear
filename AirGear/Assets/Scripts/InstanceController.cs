using UnityEngine;
using System.Collections;

public class InstanceController : MonoBehaviour 
{
	public static Player _player;
	public static PlayerCam _cam;

	public Player player
	{
		get { return _player; }
	}
	
	public PlayerCam cam
	{
		get { return _cam;}	
	}

	void Awake()
	{
		_player = GetComponent<Player>();
		_cam = GetComponent<PlayerCam>();
	}
}