using UnityEngine;
using System.Collections;

public class lever : MonoBehaviour 
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
}
