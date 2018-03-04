using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game : ParentUI {


	public Transform[] grads;
	public CanvasGroup tutorial;
	public Text tutorialText;

	void OnEnable()
	{
		EventManager.OnGameStart += OnGameStart;
		EventManager.OnTutorialEnd += OnTutorialEnd;
	}

	void OnDisable()
	{
		EventManager.OnGameStart -= OnGameStart;
		EventManager.OnTutorialEnd -= OnTutorialEnd;
	}


	void OnGameStart()
	{
		ActivateGameGradients();
		ActivateTutorial();
		Show();
	}

	void OnTutorialEnd()
	{
		tutorial.alpha = 0;
	}


	private void ActivateGameGradients()
	{

		foreach (Transform t in grads)
		{
			t.gameObject.SetActive(false);
		}

		grads[GameplayController.LevelIndex].gameObject.SetActive(true);

	}

	private void ActivateTutorial()
	{
		tutorial.alpha = 1f;
	}


	void Start()
	{
		Init();
		Hide();
	}
}
