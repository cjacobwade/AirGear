using UnityEngine;
using System.Collections;

public class InstanceController : MonoBehaviour 
{
	public static Player _player;

	public Player player
	{
		get { return _player; }
	}
	
	public Camera _camera;

	void Awake()
	{
		_player = GetComponent<Player>();
		_camera = GetComponent<Camera>();
	}
}