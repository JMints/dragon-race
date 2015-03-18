using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class move_opponent : move_player
{
	protected override void Start()
	{
		//These values will multiply the current speed
		//by the given amount; i.e. slow_obs_small = 0.8F
		//means that when hitting a small obstacle
		//we change to 80% of our current speed.
		slow_obs_large = 0.6F;
		slow_obs_small = 0.8F;
		slow_wall = 0.7F;
		
		speed_y_max = -9F; //This enemy is a little slower than the player
		speed_x_max = 10F;
		
		y_accelerate = 1.5F;
		y_coefficient = 1F;
		
		permission_to_fly = false;
	}

	//Use FixedUpdate for physics stuff vs normal Update
	protected override void FixedUpdate() 
	{
		List<GameObject> obstacles_c = getObstacles ();
		GameObject lever_c = getLever ();
		float direction_x = AI (gameObject, obstacles_c, lever_c); //How direction_x is determined at any given step will be essentially be the AI

		if(permission_to_fly)
			rigidbody2D.velocity = new Vector2 (direction_x * speed_x_max, y_coefficient * speed_y_max);

		//always down key
		Acceleration();

		//Set initial fly permission
		if (Input.GetKeyDown (KeyCode.DownArrow) == true)
			permission_to_fly = true; //sloppy - hits every time after as well
	}

	//Gets obstacles on right side of screen
	private List<GameObject> getObstacles()
	{
		List<GameObject> obs = new List<GameObject>();
		GameObject[] holder;
		int x_division = 0; //used as halfway marker to see if obstacles are on the right side

		holder = GameObject.FindGameObjectsWithTag ("obs_large"); //get all objs w/ tag
		foreach (GameObject g in holder) 
		{
			if(g.transform.position.x > x_division)
			{
				if (g.GetComponent<SpriteRenderer> ().isVisible)
					obs.Add (g);
			}
		}

		holder = GameObject.FindGameObjectsWithTag ("obs_small");
		foreach (GameObject g in holder) 
		{
			if(g.transform.position.x > x_division)
			{
				if (g.GetComponent<SpriteRenderer> ().isVisible)
					obs.Add (g);
			}
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

	private GameObject getLever()
	{
		List<GameObject> levers = new List<GameObject>();
		GameObject[] holder;
		int x_division = 0; //used as halfway marker to see if obstacles are on the right side
		
		holder = GameObject.FindGameObjectsWithTag ("lever"); //get all objs w/ tag
		foreach (GameObject g in holder) 
		{
			if(g.transform.position.x > x_division)
			{
				if (g.GetComponent<SpriteRenderer> ().isVisible)
					levers.Add (g);
			}
		}
		
		//As obstacle types grow, include their tags here.
		
		//Sort by y-coordinate
		levers = levers.OrderBy (g => g.transform.position.y).ToList ();
		
		/*Test code
		foreach (GameObject g in obs) 
		{
			Debug.Log (g.transform.position.y);
		}*/

		if (levers.Count > 0)
			return levers[0];

		else return null;
	}
	
	private float AI(GameObject body, List<GameObject> obstacles, GameObject lever)
	{
		//Runs through each AI case to determine: 
		//Do I accelerate? AND Do I move left, right, or neither?
		//Returns a float indicating x-direction (1 is right, -1 is left, 0 is straight)

		float x_direction = 0F;

		//No obstacles or switch
		if (obstacles.Count == 0) //No elements in AL, should mean no obstacles
		{ 
			if (lever == null) //No switch
			{
				x_direction = 0F;
				//Accelerate
			}

			else
				x_direction = leverLogic(gameObject, lever);
		}

		else
		{
			bool collideLeft = false;
			bool collideRight = false;

			//Check to see if we will collide with an obstacle
			//EVENTUALLY: Extract colliding obstacles
			foreach (GameObject obstacle in obstacles)
			{
				bool temp = false; //Used to store individuals, whereas collideleft/right should be toggled once
				temp = collideCheck(leftSide(gameObject), leftSide(obstacle), rightSide(obstacle));
				if(temp)
					collideLeft = true;

				temp = collideCheck(rightSide(gameObject), leftSide(obstacle), rightSide(obstacle));
				if(temp)
					collideRight = true;
			}

			//Go for the switch if it exists and comes before any obstacles
			bool validLever = false;
			if(lever != null)
				validLever = leverIsFirst(lever, obstacles[0]);

			if (validLever)
				x_direction = leverLogic(gameObject, lever);

			//Avoid obstacles
			else if (collideLeft && collideRight)
				x_direction = insideLogic(gameObject, obstacles[0]);

			else if (collideLeft)
			{
				if(!wallCheckLeft())
					x_direction = 1F;
				else
					x_direction = -1F;
			}

			else if (collideRight)
			{
				if(!wallCheckRight())
					x_direction = -1F;
				else
					x_direction = 1F;
			}

			else 
				x_direction = 0;
				//accel
		}

		return x_direction;
	}

	//Compare lever to body and head towards it
	private float leverLogic(GameObject body, GameObject lever)
	{
		float compare = body.transform.position.x - lever.transform.position.x;
		if (compare >= 0)
			return -1F;
		else
			return 1F;
	}

	//These functions return the left-most/right-most position rendered
	//from an object's default center
	private float leftSide(GameObject obj)
	{
		SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
		float width = renderer.bounds.size.x;

		return obj.transform.position.x - (0.5F * width);
	}

	private float rightSide(GameObject obj)
	{
		SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
		float width = renderer.bounds.size.x;
		
		return obj.transform.position.x + (0.5F * width);
	}

	//Checks to see if an int lies in a range
	//Used to see if part of a player lies within obstacle bounds
	private bool collideCheck(float dragon, float low, float high)
	{
		if (dragon >= low && dragon <= high)
			return true;
		else
			return false;
	}

	//Sees if the first object is "higher" than the second
	//Used to determine if a lever is higher than the next obstacle
	bool leverIsFirst(GameObject lever, GameObject obstacle)
	{
		bool result = false;
		if (lever.transform.position.y > obstacle.transform.position.y)
			result = true;
		return result;
	}

	//Compares the relative centers of two objects
	//Returns positive if first center is right of second, negative if to left
	//Used to determine dodge direction when both borders collide
	private float insideLogic(GameObject body, GameObject obstacle)
	{
		float compare = body.transform.position.x - obstacle.transform.position.x;
		if (compare >= 0)
			return 1F;
		else
			return -1F;
	}

	//Edge case functions, work in later along with side obs check
	private bool wallCheckLeft()
	{
		return false;
	}

	private bool wallCheckRight()
	{
		return false;
	}
}
