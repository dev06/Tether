using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreHandler : MonoBehaviour {


	private Text scoreText;
	private Color defaultColor;
	private Vector3 defaultScale;
	private float scaleMultipler = 1;
	private bool inBoost;

	public Color boostColor = Color.white;
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
		scoreText = GetComponent<Text>();
		defaultColor = scoreText.color;
		defaultScale = transform.localScale;
		scoreText.text = GameplayController.SCORE.ToString();
	}
	void Start ()
	{
		Init();
	}

	void Update ()
	{
		transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.deltaTime * 10f);
	}

	void OnGameStart()
	{
		Init();
	}

	private void OnBaseHit()
	{
		scoreText.text = (GameplayController.SCORE).ToString();
		scaleMultipler = inBoost ? 4.5f : 1.5f;
		Vector3 addition = defaultScale * scaleMultipler;
		transform.localScale = addition;
	}

	private void OnBoostStart()
	{
		SetColor(boostColor);
		inBoost = true;
	}


	private void OnBoostEnd()
	{
		SetColor(defaultColor);
		inBoost = false;
	}

	public void SetColor(Color c)
	{
		scoreText.color = c;
	}
}
