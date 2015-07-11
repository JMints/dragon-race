using UnityEngine;
using System.Collections;

public class move_camera : MonoBehaviour 
{
	//+int camera_offset = -10;
	public bool scrolled;
	KeyCode key_skip;

	//Set in editor
	GameObject player;
	public GameObject start_line;
	public GameObject finish_line;

	//Camera panning vars
	float start;
	float finish;
	float position;
	bool down;

	void Start ()
	{
		player = GameObject.Find ("player");

		scrolled = true;
		key_skip = KeyCode.Space;

		start = start_line.transform.position.y;
		finish = finish_line.transform.position.y;
		position = start;
		down = true;
	}

	// Update is called once per frame
	void Update () 
	{

		//Allows us to skip scrolling script with key press
		if (Input.GetKeyUp (key_skip))
			scrolled = true;

		if (!scrolled)
		{
			//Scroll Down
			if(down)
			{
				position = position - 0.5F; //Number affects speed of scrolling
				transform.localPosition = new Vector3 (transform.position.x, position, transform.position.z);

				//Stop going down when we get to the finish line.
				if (position <= finish)
					down = false;
			}

			//Scroll Up
			else
			{
				position = position + 2F; //Number affects speed of scrolling
				transform.localPosition = new Vector3 (transform.position.x, position, transform.position.z);

				//Stop scrolling when we get to the starting line.
				if (position >= start)
					scrolled = true;
			}
		}
		//Issue with camera "snapback" due to jump to camera offset

		//Game shouldn't start until the pan is finished -
		//have scrolled affect permissiontofly in move_player

		//Add key press to skip scrolling script

		if (scrolled)
		{
			float speed = player.GetComponent<move_player>().speed_y_max * player.GetComponent<move_player>().y_coefficient * Time.deltaTime;
			if(!player.GetComponent<move_player>().permission_to_fly)
				speed = 0;
			//Set camera to an offset of the current player location
			transform.localPosition = new Vector3 (transform.position.x, transform.position.y + speed, transform.position.z);
		}
	}
}
