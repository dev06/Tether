using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game : ParentUI {


	public Transform[] grads;
	public CanvasGroup tutorial;
	public Text tutorialText;
	public Image backgroundNeg;
	public Image pauseButton;

	void OnEnable()
	{
		EventManager.OnGameStart += OnGameStart;
		EventManager.OnTutorialEnd += OnTutorialEnd;
		EventManager.OnBoostStart += OnBoostStart;
		EventManager.OnBoostEnd += OnBoostEnd;
	}

	void OnDisable()
	{
		EventManager.OnGameStart -= OnGameStart;
		EventManager.OnTutorialEnd -= OnTutorialEnd;
		EventManager.OnBoostStart -= OnBoostStart;
		EventManager.OnBoostEnd -= OnBoostEnd;
	}

	void OnBoostEnd()
	{
		backgroundNeg.enabled = false;
		pauseButton.enabled = true;
	}

	void OnBoostStart()
	{
		backgroundNeg.enabled = true;
		pauseButton.enabled = false;
	}


	void OnGameStart()
	{
		ActivateGameGradients();
		ActivateTutorial();
		Show();
		backgroundNeg.enabled = false;
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
