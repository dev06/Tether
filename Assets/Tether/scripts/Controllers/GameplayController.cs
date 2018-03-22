using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
using Facebook.Unity;
public class GameplayController : MonoBehaviour
{

	public static bool GAMESTARTED = false;

	public static bool HAPTIC = true;

	public static GameplayController Instance;

	private static bool Loaded = true;

	public float LastScore = 0;

	public float BestScore = 0;

	public static float SCORE = 0;

	public static Level Level;

	public static int LevelIndex;

	public static State GAME_STATE = State.MENU;

	public static float DIFFICULTY;

	private static int POWERUP_START = 4;

	public static int POWERUP_FREQ = POWERUP_START;

	public bool DEBUG;

	public List<LockTask> lockTasks = new List<LockTask>();

	public bool hasUsedBoost;

	public bool AllTaskComplete;

	public bool inTutorial = true;

	public static bool TutorialEnabled = true;

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
		EventManager.OnGameStart += OnGameStart;
	}


	void OnDisable()
	{
		EventManager.OnGameOver -= OnGameOver;
		EventManager.OnGameStart -= OnGameStart;
	}
	void Awake()
	{
		Application.targetFrameRate = 300;

		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			DestroyImmediate(gameObject);
		}

		if (!FB.IsInitialized)
		{
			FB.Init();
		} else
		{
			FB.ActivateApp();
		}

		if (GAMESTARTED == false)
		{
			PlayerPrefs.DeleteKey("LastLevelPlayed");
		}


		GAMESTARTED = true;

		//		PlayerPrefs.DeleteAll();
		//Debug.Log("dsfs")

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		InitGameSettings();

	}

	void Start ()
	{

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

		POWERUP_FREQ = POWERUP_START;

		SCORE = 0;

		BaseController.SpawnScore = 0;

		BaseController.CURRENT_VELOCITY = BaseController.MIN_VELOCITY;

		//CheckForTetherHeadUnlocks();


		if (PlayerPrefs.HasKey("LastLevelPlayed"))
		{
			LevelIndex = PlayerPrefs.GetInt("LastLevelPlayed");

			Level = LevelIndex == 0 ? Level.LEVEL1 : Level.LEVEL2;
		}

		SetState(!Loaded ? State.INTRO : State.MENU);

		Loaded = true;
	}



	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			DEBUG = !DEBUG;
		}

	}

	void OnGameStart()
	{
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");

		if (LevelIndex == 1)
		{
			BaseController.CURRENT_VELOCITY = 200F;
		}
	}

	void OnGameOver()
	{

		PlayerPrefs.SetFloat("last", SCORE);

		CheckForTetherHeadUnlocks();

		SaveBestScore();

		int progScore = (int)SCORE;
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "game", progScore);

		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	public void CheckForTetherHeadUnlocks()
	{
		PlayerPrefs.SetInt("CurrentTetherHeadID", ShopButtonHandler.Instance.GetCurrentTetherHead().ID);
		List<TetherHead> heads = ShopButtonHandler.Instance.TetherHeads;
		for (int i = 0; i < heads.Count; i++)
		{
			if (SCORE >= heads[i].UnlockAt)
			{
				heads[i].Locked = false;
				PlayerPrefs.SetString("TetherHead_" + i, "False");
			}
		}
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

