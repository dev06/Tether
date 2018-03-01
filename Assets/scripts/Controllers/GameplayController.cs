using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayController : MonoBehaviour
{

	public static GameplayController Instance;

	private static bool Loaded = true;

	public int LastScore = 0; 

	public int BestScore = 0; 

	public static int SCORE = 0;

	public static Level Level;

	public static int LevelIndex;

	public static State GAME_STATE = State.MENU;

	public bool DEBUG;

	public static float DIFFICULTY;

	public static long VIBRATION_DURATION = 5;

	private bool isPaused;

	public List<LockTask> lockTasks = new List<LockTask>(); 

	public bool hasUsedBoost; 

	public bool AllTaskComplete;

	private LockTaskPanel locktaskpanel; 

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

		locktaskpanel = FindObjectOfType<LockTaskPanel>(); 

	}

	void OnApplicationQuit()
	{
		DeleteAll(); 
	}

	public void InitGameSettings()
	{
		Time.timeScale = 1f;
		Time.fixedDeltaTime = Time.timeScale * .02f;
		DIFFICULTY = 0F;
		LastScore = PlayerPrefs.GetInt("last"); 
		BestScore = PlayerPrefs.GetInt("best"); 
		SCORE = 0;
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

	void OnGameOver()
	{
		PlayerPrefs.SetInt("last", SCORE); 
	//	PlayerPrefs.SetString("ReverbToggle", AudioController.)
		SaveBestScore(); 
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}


	void OnGameStart()
	{

	}

	public void DeleteAll()
	{
		PlayerPrefs.DeleteAll(); 
	}


	public void IncrementScore()
	{
		SCORE++;
		DIFFICULTY = Mathf.Log(SCORE);
		DIFFICULTY = Mathf.Clamp(DIFFICULTY, 0, 10F);


		if(SCORE >= LockTaskValue.Task1Value)
		{
			if(locktaskpanel != null)
			{
				locktaskpanel.InvokeLockTask(LockTaskID.ID_1); 
			}
		}

		if(SCORE >= LockTaskValue.Task3Value && !hasUsedBoost)
		{
			if(locktaskpanel != null)
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
		if(PlayerPrefs.HasKey("best"))
		{
			int best = PlayerPrefs.GetInt("best"); 

			if(SCORE > best)
			{
				PlayerPrefs.SetInt("best", SCORE); 
			}
		}
		else
		{
			PlayerPrefs.SetInt("best", SCORE); 
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
		foreach(LockTask l in lockTasks)
		{
			if(l.taskID == id)
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

