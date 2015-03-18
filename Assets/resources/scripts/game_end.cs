using UnityEngine;
using System.Collections;

public class game_end : MonoBehaviour 
{
	void OnCollisionEnter2D(Collision2D collision) 
	{
		if (collision.gameObject.tag == "Player")
		{

			Application.LoadLevel("menu_win");
		}

		if (collision.gameObject.tag == "enemy")
		{	
			Application.LoadLevel("menu_lose");
		}
	}
}
