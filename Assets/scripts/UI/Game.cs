using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : ParentUI {


	public Transform[] grads; 

	void OnEnable()
	{
		EventManager.OnGameStart+=OnGameStart; 
	}

	void OnDisable()
	{
		EventManager.OnGameStart-=OnGameStart; 
	}


	void OnGameStart()
	{
		ActivateGameGradients(); 
		Show();
	}

	private void ActivateGameGradients()
	{

		foreach(Transform t in grads)
		{
			t.gameObject.SetActive(false); 
		}

		grads[GameplayController.LevelIndex].gameObject.SetActive(true); 

	}


	void Start()
	{
		Init();
		Hide(); 
	}
}
