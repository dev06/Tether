using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : ParentUI {

	public CanvasGroup titleOverlay;

	public Text currentTrackText;

	private LevelSelectUI tints;

	public Text scoreText; 
	void Start ()
	{
		Init();
		currentTrackText.text = AudioController.Instance.CurrentTrack();
		tints = FindObjectOfType<LevelSelectUI>();
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
		if(s == State.MENU)
		{
			scoreText.text = GameplayController.Instance.BestScore + " | " + GameplayController.Instance.LastScore; 
		}
	}

	void OnLevelChange(Level l)
	{
		currentTrackText.text = AudioController.Instance.CurrentTrack();
	}


	void Update ()
	{
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
