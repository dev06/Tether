﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : ParentUI {

	public CanvasGroup titleOverlay;

	public Text currentTrackText;

	private LevelSelectUI tints;

	public Text scoreText;

	public float sizeX, sizeY;

	private RectTransform rt;
	void Start ()
	{
		Init();
		currentTrackText.text = AudioController.Instance.CurrentTrack();
		tints = FindObjectOfType<LevelSelectUI>();
		rt = GetComponent<RectTransform>();
		sizeX = rt.rect.width;
		sizeY = rt.rect.height;


		if (EventManager.OnDisplayChange != null)
		{
			EventManager.OnDisplayChange(sizeX, sizeY);
		}

	}

	void OnEnable()
	{
		EventManager.OnStateChange += OnStateChange;
		EventManager.OnLevelChange += OnLevelChange;

	}

	void OnDisable()
	{
		EventManager.OnStateChange -= OnStateChange;
		EventManager.OnLevelChange -= OnLevelChange;
	}


	void OnStateChange(State s)
	{
		if (s == State.MENU)
		{
			scoreText.text = ((int)GameplayController.Instance.BestScore) + " | " + ((int)GameplayController.Instance.LastScore);

			Show();
		}

		if(s == State.GAME)
		{
			gameObject.SetActive(false); 
		}
	}

	void OnLevelChange(Level l)
	{
		currentTrackText.text = AudioController.Instance.CurrentTrack();
	}


	void Update ()
	{
		if (sizeX != rt.rect.width)
		{
			sizeX =  rt.rect.width;
			sizeY =  rt.rect.height;

			if (EventManager.OnDisplayChange != null)
			{
				EventManager.OnDisplayChange(sizeX, sizeY);
			}
		}

		if (GameplayController.GAME_STATE != State.MENU)
		{
			tints.transform.gameObject.SetActive(false);
			return;
		}

		if (tints.transform.gameObject.activeSelf == false)
		{
			tints.transform.gameObject.SetActive(true);
		}

	}


	public void StartGame(Level level)
	{
		Hide();

		LevelController.Instance.SetLevel(level);

		GameplayController.SetState(State.GAME);

		PlayerPrefs.SetInt("LastLevelPlayed", level == Level.LEVEL1 ? 0 : 1);

		if (EventManager.OnGameStart != null)
		{
			EventManager.OnGameStart();
		}

	}

	public virtual void Show()
	{
		base.Show();
	}

	public virtual void Hide()
	{
		base.Hide();
	}
}
