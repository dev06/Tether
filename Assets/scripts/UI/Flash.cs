using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Flash : MonoBehaviour {

	CanvasGroup group;

	void OnEnable()
	{
		EventManager.OnGameStart += OnGameStart;
	}
	void OnDisable()
	{
		EventManager.OnGameStart -= OnGameStart;
	}

	void OnGameStart()
	{
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
