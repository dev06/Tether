using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Flash : MonoBehaviour {

	CanvasGroup group;

	public Color startColor = Color.white;

	private Image image;
	void OnEnable()
	{
		EventManager.OnGameStart += OnGameStart;
		EventManager.OnStateChange += OnStateChange;
	}
	void OnDisable()
	{
		EventManager.OnGameStart -= OnGameStart;
		EventManager.OnStateChange -= OnStateChange;
	}

	void OnGameStart()
	{
	}

	void OnStateChange(State s)
	{
		if (s == State.SETTING) { return; }

		StopCoroutine("Fade");
		StartCoroutine("Fade");
	}
	void Start ()
	{
		group = GetComponent<CanvasGroup>();
		StartCoroutine("Fade");
	}

	IEnumerator Fade()
	{
		if (image == null)
		{
			image = GetComponent<Image>();
		}

		image.color = startColor;

		float alpha = 1;

		while (alpha > 0)
		{
			alpha -= Time.unscaledDeltaTime;
			group.alpha = alpha;
			yield return null;
		}

		group.blocksRaycasts = false;
	}
}
