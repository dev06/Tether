using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIFade : MonoBehaviour {


	public enum FadeType
	{
		FadeIn,
		FadeOut,
	}

	private CanvasGroup canvasGroup;
	public FadeType type;
	public State stateToStartOn;
	public float delay;
	public float speed;

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
		if (s == stateToStartOn)
		{
			switch (type)
			{
				case FadeType.FadeIn:
				{
					StopCoroutine("FadeIn");
					StartCoroutine("FadeIn");
					break;
				}
			}
		}
	}

	void Start ()
	{
		canvasGroup = GetComponent<CanvasGroup>();

	}

	IEnumerator FadeIn()
	{
		if (canvasGroup == null)
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		canvasGroup.alpha = 0;
		yield return new WaitForSeconds(delay);
		float alpha = 0;
		while (alpha < 1)
		{
			alpha += Time.unscaledDeltaTime * speed;
			canvasGroup.alpha = alpha;
			yield return null;
		}

		canvasGroup.blocksRaycasts = true;
	}

	void Update () {

	}
}

