using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialHandler : MonoBehaviour {

	void OnEnable()
	{
		EventManager.OnTutorialActive += OnTutorialActive;
	}

	void OnDisable()
	{
		EventManager.OnTutorialActive -= OnTutorialActive;
	}

	void OnTutorialActive(bool active)
	{
		UpdateSprite(active);
	}


	public Sprite tutorial_on;
	public Sprite tutorial_off;

	private Image image;


	void UpdateSprite(bool b)
	{
		if (image == null)
		{
			image = GetComponent<Image>();
		}

		image.sprite = b ? tutorial_on : tutorial_off;
	}

	void Start ()
	{
		UpdateSprite(GameplayController.TutorialEnabled);
	}
}
