using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class speed_bar : MonoBehaviour 
{
	RectTransform location;
	GameObject player;
	move_player player_script;
	float speed;
	float y_max;
	float y_min;
	float x;
	//Text value;

	// Use this for initialization
	void Start () 
	{
		location = gameObject.GetComponent<RectTransform>();
		player = GameObject.Find ("player"); //make sure player is rendering before this is called!
		player_script = (move_player)player.GetComponent (typeof(move_player));
		speed = 1; //Will update to whatever is in player_script after first frame
		y_max = location.position.y;
		y_min = location.position.y - location.rect.width;
		x = location.position.x;

		//find value
		//value.text = "100.00%"; //Will update after first frame
	}
	
	// Update is called once per frame
	void Update () 
	{
		speed = player_script.getCurrentSpeed ();
		float y_offset = (y_max - y_min) * speed;
		//transform to x_min + x_offset, y
		location.position = new Vector3 (x, (y_min + y_offset), 0); //(x_min + x_offset)
		//value.text = speed * 100 + "%";
	}
}
