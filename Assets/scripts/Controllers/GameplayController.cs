using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayController : MonoBehaviour
{

	public static GameplayController Instance;

	public static int SCORE = 0;

	public static Level Level;

	public static int LevelIndex;

	public static State GAME_STATE = State.MENU;

	public bool DEBUG;

	public static float DIFFICULTY;

	public static long VIBRATION_DURATION = 5;

	private bool isPaused; 

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

		InitGameSettings();


	}

	void Start ()
	{
		Application.targetFrameRate = 60;

		ObjectSpawner.Instance.Init();

	}

	public void InitGameSettings()
	{

		Time.timeScale = 1f;
		Time.fixedDeltaTime = Time.timeScale * .02f;
		DIFFICULTY = 0F;
		SCORE = 0;
		SetState(State.MENU);
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
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}


	void OnGameStart()
	{

	}


	public void IncrementScore()
	{
		SCORE++;
		DIFFICULTY = Mathf.Log(SCORE);
		DIFFICULTY = Mathf.Clamp(DIFFICULTY, 0, 10F);
	}

	public void TogglePause(bool b)
	{
		this.isPaused = b;  



		if(isPaused)
		{
			if(EventManager.OnPause != null)
			{
				EventManager.OnPause();
			}

			GAME_STATE = State.PAUSE; 
		}
		else if(!isPaused)
		{
			if(EventManager.OnUnpause != null)
			{
				EventManager.OnUnpause(); 
			}

			GAME_STATE = State.GAME; 
		}


	}	

	public bool Paused
	{
		get
		{
			return isPaused; 
		}
	}

}


public enum State
{
	MENU,
	GAME,
	PAUSE,
	CREDIT,
	SETTING,
}

