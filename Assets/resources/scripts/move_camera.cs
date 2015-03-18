using UnityEngine;
using System.Collections;

public class move_camera : MonoBehaviour 
{
	int camera_offset = -10;

	// Update is called once per frame
	void Update () 
	{
		//Get current speed of player
		Transform player = GameObject.Find ("player").transform;

		//Set camera to an offset of the current player location
		transform.localPosition = new Vector3 (transform.position.x, player.position.y + camera_offset, transform.position.z);
	}
}
