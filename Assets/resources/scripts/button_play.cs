using UnityEngine;
using System.Collections;

public class button_play : MonoBehaviour 
{

	void OnMouseDown()
	{
		// if we clicked the play button
		if(Input.GetMouseButtonDown(0))
		{
			// load the game
			Application.LoadLevel("level_design");
		}
	}
}
