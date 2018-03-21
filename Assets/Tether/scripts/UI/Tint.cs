using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tint : MonoBehaviour {


	Image image;
	int index;
	public Color[] levelColors;
	Color targetColor;

	void OnEnable()
	{
		EventManager.OnBaseHit += OnBaseHit;

	}

	void OnDisable()
	{
		EventManager.OnBaseHit -= OnBaseHit;
	}

	void Start ()
	{
		image = GetComponent<Image>();
	}

	void Update()
	{
		Color c = Color.Lerp(image.material.GetColor("_Color"), targetColor, Time.unscaledDeltaTime * 5f);
		image.material.SetColor("_Color", c);
	}


	void OnBaseHit ()
	{
		if ((int)GameplayController.SCORE % 3 == 0)
		{
			if (index > levelColors.Length - 1)
			{
				index = 0;
			}

			targetColor = levelColors[index];
			index++;
			//image.material.SetColor("_Color", levelColors[index]);
		}
	}
}
