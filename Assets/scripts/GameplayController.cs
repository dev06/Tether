using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour {

	public static GameplayController Instance;

	public static int SCORE = 0;
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

	public void IncrementScore()
	{
		SCORE++;
	}
}
