using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : ParentUI {



	void OnEnable()
	{
		EventManager.OnGameStart+=OnGameStart; 
		//EventManger.OnGameOver-=OnGameS; 


	}

	void OnDisable()
	{
		EventManager.OnGameStart-=OnGameStart; 
	}


	void OnGameStart()
	{
		Show(); 
	}
	void Start()
	{
		Init();
		Hide(); 
	}
}
