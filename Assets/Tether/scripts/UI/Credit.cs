using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : ParentUI
{

	private LevelSelectUI tints;
	private Animation animation;

	void Start ()
	{
		Init();
		Hide();
		tints = FindObjectOfType<LevelSelectUI>();
		animation = GetComponent<Animation>();
	}

	void OnEnable()
	{
		EventManager.OnStateChange += OnStateChange;
	}

	void OnDisable()
	{
		EventManager.OnStateChange -= OnStateChange;
	}


	void OnStateChange(State s)
	{
		if (tints == null)
		{
			tints = FindObjectOfType<LevelSelectUI>();
		}

		//tints.transform.gameObject.SetActive(false);

		if (s == State.GAME)
		{
			gameObject.SetActive(false); 			
		}

		if (s != State.CREDIT) {
			Hide();
			return;
		}

		animation.Play();
		Show();
	}
}
