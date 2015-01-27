using UnityEngine;
using System.Collections;

public class lever_script : MonoBehaviour 
{
	//New Logic - Contains literal switch for switch blocks to check
	private bool isActivated; //Has this switch been pressed?

	//False at start
	private void Start()
	{
		isActivated = false;
	}

	//When pressed, becomes true.
	void OnCollisionEnter2D(Collision2D collision)
	{
		isActivated = true;
		Destroy (gameObject);
	}

	//Returns current value to obstacles.
	public bool getSwitch()
	{
		return isActivated;
	}

	/* Old logic - drew individual blocks when switch hit
	GameObject source;
	Vector3 position_1; //Need one for each obj for the switch
						//May inherit from other class where we determine what locations and which switches
	void Start()
	{
		source = GameObject.Find ("obs_small_1");
		position_1 = new Vector3 (4, 20, 0);
	}


	//Separate function called for every kind of obstacle; handles slowdown
	void OnCollisionEnter2D(Collision2D collision)
	{
		//need example/source objects
		Instantiate(source, position_1, Quaternion.identity);
		Destroy (gameObject);
	}*/
}
