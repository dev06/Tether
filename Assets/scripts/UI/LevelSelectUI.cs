﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelectUI : MonoBehaviour {


	public CanvasScaler scaler;

	public Transform LevelSelectHUD;

	private Vector3 lastMousePosition;

	private Vector3 mousePositionDown;

	private Vector3 mousePositionUp;

	private float targetPosition;

	private float nextSwipeThreshold = .2f;

	private float tapThreshold = 20f;

	private float swipeSmoothTime = 1.5f;

	private float distanceVel;

	private float delta;

	private int holdIndex;

	private bool isHolding;

	private int index;

	private float range;

	private float hudDefaultScale = .15f;

	private float hudSelectedScale = .25f;

	private Menu menu;

	private LevelController levelController;

	private float controlTimer = 0;

	private float controlTimerThreshold = .4f;

	private SettingPanel settingPanel;

	private bool canStartGame = true;

	private float magnitude;

	private bool controlOnStateChange;

	private Vector2 displacement;

	private float screen;


	void Start ()
	{
		menu = FindObjectOfType<Menu>();

		settingPanel = FindObjectOfType<SettingPanel>();

		levelController = LevelController.Instance;

		screen =scaler.referenceResolution.x; 
	}

	void OnEnable()
	{

		EventManager.OnStateChange += OnStateChange;
		EventManager.OnGameStart += OnGameStart;
		EventManager.OnGameOver += OnGameOver;


	}

	void OnDisable()
	{

		EventManager.OnStateChange -= OnStateChange;
		EventManager.OnGameStart -= OnGameStart;
		EventManager.OnGameOver -= OnGameOver;


	}

	void OnStateChange(State s)
	{
		if (s == State.MENU)
		{
			StopCoroutine("IStartControlTimer");
			StartCoroutine("IStartControlTimer");
			controlOnStateChange = true;
		}

		isHolding = false;
	}

	void OnGameOver()
	{
	}
	void OnGameStart()
	{

	}







	private IEnumerator IStartControlTimer()
	{
		controlTimer = 0;

		while (controlTimer < 1)
		{
			controlTimer += Time.unscaledDeltaTime;
			yield return null;
		}
	}


	void Update ()
	{

		if (GameplayController.GAME_STATE != State.MENU) { return; }

		// if (controlTimer < controlTimerThreshold)
		// {
		// 	isHolding = false;
		// 	return;
		// }

		if (Input.GetMouseButtonDown(0))
		{

			isHolding = true;

			holdIndex = index;

			mousePositionDown = Input.mousePosition;

			lastMousePosition = Input.mousePosition;

			displacement = Vector2.zero;
		}



		if (isHolding)
		{
			displacement += (Vector2)lastMousePosition - (Vector2)Input.mousePosition;

			displacement.x = Mathf.Abs(displacement.x);

			if (Mathf.Abs(displacement.x) > Screen.width * .01f)
			{
				canStartGame = false;
			}
			//Debug.Log(displacement );
		}

		if (Input.GetMouseButtonUp(0))
		{

			canStartGame = true;
			mousePositionUp = Input.mousePosition;

			delta = mousePositionUp.x - mousePositionDown.x;


			isHolding = false;

			magnitude = Mathf.Abs(delta);





			if (magnitude < nextSwipeThreshold * Screen.width) {

				range = -holdIndex * Screen.width;



				return;
			}

			index = delta < 0 ? index + 1 : delta > 0 ? index - 1 : index;

			index = Mathf.Clamp(index, 0, transform.childCount);

			range = -index * Screen.width;


			Level l = levelController.ParseLevel(index);


			if (EventManager.OnLevelChange != null)
			{

				EventManager.OnLevelChange(l);
			}

			GameplayController.Level = l;
			GameplayController.LevelIndex = index;
			//			SwitchPanel(ref index);

		}

		if (isHolding)
		{
			range += (( Input.mousePosition.x - lastMousePosition.x));
		}


		targetPosition = Mathf.SmoothDamp(targetPosition, range, ref distanceVel, Time.unscaledDeltaTime * swipeSmoothTime);

		transform.localPosition = new Vector2((targetPosition * screen) / Screen.width, 0);

		float xClamp = Mathf.Clamp(transform.localPosition.x, -transform.childCount * screen, 0);

		transform.localPosition = new Vector2(xClamp, 0);

		if (isHolding)
		{
			lastMousePosition = Input.mousePosition;
		}


	}

	private void SwitchPanel()
	{
		index = delta < 0 ? index + 1 : delta > 0 ? index - 1 : index;

		index = Mathf.Clamp(index, 0, transform.childCount);

		range = -index * Screen.width;
	}

	public void StartGame()
	{

		Level level = index == 0 ? Level.LEVEL1 : Level.LEVEL2;

		bool canStart = true;

		if (level == Level.LEVEL2)
		{
			LockTaskPanel panel = FindObjectOfType<LockTaskPanel>();

			canStart = !panel.Active;
		}

		if (!canStart || !canStartGame) { return; }

		levelController.SetLevel( Level.LEVEL1);

		transform.gameObject.SetActive(false);

		GameplayController.LevelIndex = index;



		menu.StartGame(level);


	}


	private void UpdateLevelHUDScale()
	{
		if (LevelSelectHUD == null)
		{
			Debug.LogError("Level Select HUD is null.");
			return;
		}
	}

	public int LevelIndex
	{
		get
		{
			return index;
		}
	}

}
