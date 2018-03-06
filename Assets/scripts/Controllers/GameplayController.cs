﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayController : MonoBehaviour
{

	public static GameplayController Instance;

	private static bool Loaded;



	public float LastScore = 0;

	public float BestScore = 0;

	public static float SCORE = 0;

	public static Level Level;

	public static int LevelIndex;

	public static State GAME_STATE = State.MENU;

	public static float DIFFICULTY;

	public static int POWERUP_FREQ = 15;

	public bool DEBUG;

	public List<LockTask> lockTasks = new List<LockTask>();

	public bool hasUsedBoost;

	public bool AllTaskComplete;

	public bool inTutorial = true;

	public bool boostActive;

	public UnityEngine.Rendering.SortingGroup sortingGroup;

	private LockTaskPanel locktaskpanel;

	private bool isPaused;

	private PlayerController player;

	public static void SetState(State s)
	{
		GAME_STATE = s;

		if (EventManager.OnStateChange != null)
		{
			EventManager.OnStateChange(GAME_STATE);
		}
	}

	void OnEnable()
	{
		EventManager.OnGameOver += OnGameOver;
	}


	void OnDisable()
	{
		EventManager.OnGameOver -= OnGameOver;
	}
	void Awake()
	{

		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			DestroyImmediate(gameObject);
		}

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		InitGameSettings();

	}

	void Start ()
	{
		Application.targetFrameRate = 60;

		ObjectSpawner.Instance.Init();

		player = PlayerController.Instance;

		locktaskpanel = FindObjectOfType<LockTaskPanel>();

	}


	public void InitGameSettings()
	{

		Time.timeScale = 1f;

		Time.fixedDeltaTime = Time.timeScale * .02f;

		DIFFICULTY = 0F;

		LastScore = PlayerPrefs.GetFloat("last");

		BestScore = PlayerPrefs.GetFloat("best");

		SCORE = 0;

		BaseController.SpawnScore = 0;

		SetState(!Loaded ? State.INTRO : State.MENU);

		Loaded = true;
	}



	// void Update ()
	// {
	// 	if (Input.GetKeyDown(KeyCode.Space))
	// 	{
	// 		DEBUG = !DEBUG;
	// 	}
	// }

	void OnGameOver()
	{

		PlayerPrefs.SetFloat("last", SCORE);

		SaveBestScore();

		UnityEngine.SceneManagement.SceneManager.LoadScene(0);

		//	DeleteAll();
	}


	public void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}


	public void IncrementScore()
	{
		if (inTutorial)
		{
			if (SCORE >= 1)
			{
				if (EventManager.OnTutorialEnd != null)
				{
					EventManager.OnTutorialEnd();
				}

				inTutorial = false;
			}
		}

		if (!player.activeBoost)
		{
			SCORE++;
		}

		DIFFICULTY = Mathf.Log(SCORE);

		DIFFICULTY = Mathf.Clamp(DIFFICULTY, 0, 10F);


		if (SCORE >= LockTaskValue.Task1Value)
		{
			if (locktaskpanel != null)
			{
				locktaskpanel.InvokeLockTask(LockTaskID.ID_1);
			}
		}

		if (SCORE >= LockTaskValue.Task3Value && !hasUsedBoost)
		{
			if (locktaskpanel != null)
			{
				locktaskpanel.InvokeLockTask(LockTaskID.ID_3);
			}
		}
	}

	public void TogglePause(bool b)
	{
		this.isPaused = b;

		if (isPaused)
		{
			if (EventManager.OnPause != null)
			{
				EventManager.OnPause();
			}

			GAME_STATE = State.PAUSE;

		}
		else if (!isPaused)
		{
			if (EventManager.OnUnpause != null)
			{
				EventManager.OnUnpause();
			}

			GAME_STATE = State.GAME;
		}

	}



	public void SaveBestScore()
	{
		if (PlayerPrefs.HasKey("best"))
		{
			float best = PlayerPrefs.GetFloat("best");

			if (SCORE > best)
			{
				PlayerPrefs.SetFloat("best", SCORE);
			}
		}
		else
		{
			PlayerPrefs.SetFloat("best", SCORE);
		}
	}

	public bool Paused
	{
		get
		{
			return isPaused;
		}
	}

	public LockTask GetLockTask(LockTaskID id)
	{
		foreach (LockTask l in lockTasks)
		{
			if (l.taskID == id)
			{
				return l;
			}
		}

		return null;
	}

}


public enum State
{
	MENU,
	GAME,
	PAUSE,
	CREDIT,
	SETTING,
	INTRO,
}

