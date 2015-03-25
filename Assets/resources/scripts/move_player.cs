using UnityEngine;
using System.Collections;

public class move_player : MonoBehaviour 
{
	//When testing, modify numbers from within unity, not the script directly.
	protected float slow_wall; //% slowed when a wall is hit
	public float slow_obs_large; //%slowed when big box is hit
	public float slow_obs_small; //% slowed when small box is hit

	public float speed_y_max; //this is the speed you start at
	public float speed_x_max;
	
	public float y_accelerate; //Determines how fast we accelerate back to max speed
	public float y_coefficient; //% of max horizontal speed

	protected bool permission_to_fly;
	public Camera camera_main; //Used to determine if camera still panning.

	protected virtual void Start()
	{
		//These values will multiply the current speed
		//by the given amount; i.e. slow_obs_small = 0.8F
		//means that when hitting a small obstacle
		//we change to 80% of our current speed.
		slow_obs_large = 0.6F;
		slow_obs_small = 0.8F;
		slow_wall = 0.7F;

		speed_y_max = -20F;
		speed_x_max = 17F;

		y_accelerate = 1.5F;
		y_coefficient = 1F;

		permission_to_fly = false;
	}
	
	//If we aren't at max speed, linearly accelerate back to max
	protected void Acceleration()
	{
		if(y_coefficient < 1)
		{
			y_coefficient += 0.001F * y_accelerate;
		}
		
		//x_coefficient should never become > 1
		if(y_coefficient > 1)
		{
			y_coefficient = 1;
		}
	}
	
	//Use FixedUpdate for physics stuff vs normal Update
	protected virtual void FixedUpdate() 
	{
		float direction_x = Input.GetAxis ("Horizontal");

		if(permission_to_fly)
			GetComponent<Rigidbody2D>().velocity = new Vector2 (direction_x * speed_x_max, y_coefficient * speed_y_max);

		//Set initial fly permission
		if (camera_main.GetComponent<move_camera>().scrolled) //Only works if camera done scrolling
		{
			if (Input.GetKeyDown (KeyCode.DownArrow) == true)
				permission_to_fly = true; //sloppy - hits every time after as well
		}

		//Accelerate if the down arrow key is pressed during this frame
		if ( Input.GetKey(KeyCode.DownArrow) == true )
			Acceleration();
	}
	
	//Separate function called for every kind of obstacle; handles slowdown
	protected void OnCollisionEnter2D(Collision2D collision)
	{
		wall_collide (collision);
		check_y (); //One check_y is sufficient per frame
	}

	protected void OnTriggerEnter2D(Collider2D collision)
	{
		obs_large_collide (collision);
		obs_small_collide (collision);
		check_y (); //One check_y is sufficient per frame
	}

	protected void check_y()
	{		
		//y_coefficient should never become < 0
		//Change this to reflect what percent of the max speed
		//is the minimum (NOTE: diff from value min speed.)
		if(y_coefficient < 0.2F)
		{
			y_coefficient = 0.2F;
		}
	}

	protected void obs_large_collide(Collider2D collision)
	{
		if (collision.gameObject.tag == "obs_large")
		{
			y_coefficient *= slow_obs_large;
			Destroy(collision.gameObject);
		}
	}
	
	protected void obs_small_collide(Collider2D collision)
	{
		if (collision.gameObject.tag == "obs_small")
		{
			y_coefficient *= slow_obs_small;
			Destroy (collision.gameObject);
		}
	}

	protected void wall_collide(Collision2D collision)
	{
		if (collision.gameObject.tag == "wall")
		{
			y_coefficient *= slow_wall;
		}
	}

	//Returns y_coefficient to UI.
	public float getCurrentSpeed()
	{
		return y_coefficient;
	}
}
