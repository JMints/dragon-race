using UnityEngine;
using System.Collections;

public class obs_toggle_a : MonoBehaviour 
{
	private SpriteRenderer sprite_renderer;
	private BoxCollider2D box;
	private GameObject lever;
	private lever script;

	void Start () //don't render switch objects by default
	{
		sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
		sprite_renderer.enabled = false;

		box = gameObject.GetComponent<BoxCollider2D> ();
		box.enabled = false;

		//ATTENTION TROGLODYTES: If making a new switch, create a new file obs_toggle_X, where X is the new letter.
		//Then copy-paste this and change the letter below so it matches your new switch.
		//Make sure all your obs for the new switch use this script, and not one for a diff switch.
		lever = GameObject.Find ("lever_a");
		script = (lever)lever.GetComponent (typeof(lever));
	}

	void Update () //If switch is pressed, render!
	{
		if (script.getSwitch())
		{
			sprite_renderer.enabled = true;
			box.enabled = true;
		}
	}
}
