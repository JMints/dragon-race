using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class game_pause : MonoBehaviour 
{
	//This class handles all the functionality for the pause menu,
	//as well as binding the pause menu to a key in-game.
	KeyCode key_skip;
	public Canvas canvas_pause;


	// Use this for initialization
	void Start () 
	{
		key_skip = KeyCode.Escape;
		//canvas_pause initialized in editor
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (key_skip)) 
		{
			pause();
		}
	}

	public void pause()
	{
		//If playing, pause;
		if (Time.timeScale == 1.0F)
		{
			Time.timeScale = 0.0F;
			canvas_pause.enabled = true;
		}
		
		//Else unpause
		else
		{
			resume();
		}
	}

	public void resume()
	{
		Time.timeScale = 1.0F;
		canvas_pause.enabled = false;
	}

	public void menu_main()
	{
		Application.LoadLevel ("menu_main");
	}

	public void quit()
	{
		Application.Quit (); //DOES NOT WORK IN EDITOR
	}
}
