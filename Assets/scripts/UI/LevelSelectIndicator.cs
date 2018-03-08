using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectIndicator : MonoBehaviour {


	public LevelSelectUI selector;

	private Vector3 defaultScale;

	private Vector3 hoverScale;

	private float smoothTime = 10f;


	void Start ()
	{
		defaultScale = transform.localScale;

		hoverScale = defaultScale * 1.2f;

	}

	void Update ()
	{
		if (selector.LevelIndex == transform.GetSiblingIndex())
		{
			transform.localScale = Vector3.Lerp(transform.localScale, hoverScale, Time.unscaledDeltaTime * smoothTime);
		}
		else
		{
			transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.unscaledDeltaTime * smoothTime);
		}
	}
}
