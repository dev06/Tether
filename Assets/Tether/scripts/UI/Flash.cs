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
		EventManager.OnStateChange += OnStateChange;
	}
	void OnDisable()
	{
		EventManager.OnStateChange -= OnStateChange;
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
		StopCoroutine("Fade");
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
