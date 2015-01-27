using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class move_opponent_script : move_player_script 
{
	protected override void Start()
	{
		slow_obs_large = -0.2F;
		slow_obs_small = -0.4F;
		slow_wall = -0.3F;

		speed_y_max = -10F; //This enemy is a little slower than the player
		speed_x_max = 10F;
		
		y_accelerate = 1F;
		y_coefficient = 1F;

		permission_to_fly = false;

	}

	//Use FixedUpdate for physics stuff vs normal Update
	protected override void FixedUpdate() 
	{
		getObstacles ();
		float direction_x = 0; //How direction_x is determined at any given step will be essentially be the AI

		if(permission_to_fly)
			rigidbody2D.velocity = new Vector2 (direction_x * speed_x_max, y_coefficient * speed_y_max);

		//always down key
		Acceleration();

		//Set initial fly permission
		if (Input.GetKeyDown (KeyCode.DownArrow) == true)
			permission_to_fly = true; //sloppy - hits every time after as well
	}

	//Gets ALL obstacles on screen
	//(including players - find other solution if problem)
	protected List<GameObject> getObstacles()
	{
		List<GameObject> obs = new List<GameObject>();
		GameObject[] holder;

		holder = GameObject.FindGameObjectsWithTag ("obs_large"); //get all objs w/ tag
		foreach (GameObject g in holder) 
		{
			if (g.GetComponent<SpriteRenderer> ().isVisible) //True if seen by camera
				obs.Add (g);
		}

		holder = GameObject.FindGameObjectsWithTag ("obs_small");
		foreach (GameObject g in holder) 
		{
			if (g.GetComponent<SpriteRenderer> ().isVisible)
				obs.Add (g);
		}

		//As obstacle types grow, include their tags here.

		//Sort by y-coordinate
		obs = obs.OrderBy (g => g.transform.position.y).ToList ();

		/*Test code
		foreach (GameObject g in obs) 
		{
			Debug.Log (g.transform.position.y);
		}*/

		return obs;
	}
	
	protected float AI(GameObject body, List<GameObject> obstacles, GameObject lever)
	{
		//Runs through each AI case to determine: 
		//Do I accelerate? AND Do I move left, right, or neither?
		//Returns a float indicating x-direction (1 is right, -1 is left, 0 is straight)

		float x_direction = 0;

		if (obstacles.Count == 0) //No elements in AL, should mean no obstacles
		{ 
			if (lever == null) //No switch
			{
				x_direction = 0;
			}
		}

		return x_direction;
	}
}
