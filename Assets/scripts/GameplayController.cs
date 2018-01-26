using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour {

	public static GameplayController Instance;

	public static int SCORE = 0;

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
		} else
		{
			DestroyImmediate(gameObject);
		}
	}

	void Start ()
	{

	}

	void Update () {
	}

	void OnGameOver()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		SCORE = 0;
	}


	public void IncrementScore()
	{
		SCORE++;
	}
}
