using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : ParentUI {

	public CanvasGroup titleOverlay;

	void Start ()
	{
		Init();
		Show();
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
	}


	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.U))
		{
			GameplayController.SetState(State.CREDIT); 
		}
		if (GameplayController.GAME_STATE != State.MENU) { return; }

	}


	public void StartGame(Level level)
	{
		Hide();

		LevelController.Instance.SetLevel(level);

		GameplayController.SetState(State.GAME);

		if (EventManager.OnGameStart != null)
		{
			EventManager.OnGameStart();
		}

	}

	public virtual void Show()
	{
		base.Show();

		titleOverlay.alpha = 1;

		titleOverlay.blocksRaycasts = true;
	}

	public virtual void Hide()
	{
		base.Hide();
		titleOverlay.alpha = 0;

		titleOverlay.blocksRaycasts = false;
	}
}
