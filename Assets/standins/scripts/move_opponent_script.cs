using UnityEngine;
using System.Collections;

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
		float direction_x = 0; //How direction_x is determined at any given step will be essentially be the AI

		if(permission_to_fly)
			rigidbody2D.velocity = new Vector2 (direction_x * speed_x_max, y_coefficient * speed_y_max);

		//always down key
		Acceleration();

		//Set initial fly permission
		if (Input.GetKeyDown (KeyCode.DownArrow) == true)
			permission_to_fly = true; //sloppy - hits every time after as well
	}
}
