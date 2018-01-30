using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class Menu : ParentUI {

	void Start () 
	{
		Init(); 
	}

	void OnEnable()
	{
		EventManager.OnStateChange+=OnStateChange; 

	}

	void OnDisable()
	{
		EventManager.OnStateChange-=OnStateChange; 
	}
	
	
	void OnStateChange(State s)
	{	
	}


	void Update () 
	{
		if(GameplayController.GAME_STATE != State.MENU) return;

		if(Input.GetMouseButtonUp(0))
		{
			Hide(); 

			GameplayController.SetState(State.GAME);


			if(EventManager.OnGameStart != null)
			{
				EventManager.OnGameStart(); 
			}
		}	
	}
}
