using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreHandler : MonoBehaviour {


	[HideInInspector]
	public Text scoreText;
	private Color defaultColor;
	private Vector3 defaultScale;
	private float scaleMultipler = 1;
	private bool inBoost;
	private PlayerController player;
	private float speed;
	public Color boostColor = Color.white;
	GameplayController gameplayController;
	bool hasStarted;
	void OnEnable()
	{
		EventManager.OnBaseHit += OnBaseHit;
		EventManager.OnBoostEnd += OnBoostEnd;
		EventManager.OnBoostStart += OnBoostStart;
		EventManager.OnGameStart += OnGameStart;
	}
	void OnDisable()
	{
		EventManager.OnBaseHit -= OnBaseHit;
		EventManager.OnBoostEnd -= OnBoostEnd;
		EventManager.OnBoostStart -= OnBoostStart;
		EventManager.OnGameStart -= OnGameStart;

	}

	void Init()
	{
		player = PlayerController.Instance;
		scoreText = GetComponent<Text>();
		defaultColor = scoreText.color;
		defaultScale = transform.localScale;
		gameplayController = GameplayController.Instance;
		scoreText.text = GameplayController.SCORE.ToString();
	}
	void Start ()
	{
		Init();

		//PlayerPrefs.DeleteAll();
	}

	void Update ()
	{
		if (GameplayController.GAME_STATE != State.GAME) { return; }
		speed = player.activeBoost ? 7f : 10f;
		transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.unscaledDeltaTime * speed);
	}

	void OnGameStart()
	{
		Init();
	}

	private void OnBaseHit()
	{
		scoreText.text = ((int)(GameplayController.SCORE)).ToString();
		scaleMultipler = inBoost ? 5.0f : 2.0f;
		Vector3 addition = defaultScale * scaleMultipler;
		transform.localScale = addition;
	}

	private void OnBoostStart()
	{
		GameplayController.SCORE += 1;
		scoreText.text = ((int)GameplayController.SCORE).ToString();
		SetColor(defaultColor);
		inBoost = true;
	}


	private void OnBoostEnd()
	{
		SetColor(defaultColor);
		GameplayController.SCORE = player.IdealScore;
		scoreText.text = ((int)GameplayController.SCORE).ToString();
		inBoost = false;
	}

	public void SetColor(Color c)
	{
		scoreText.color = c;
	}
}
