using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HapticHandler : MonoBehaviour {


	void OnEnable()
	{
		EventManager.OnHapticVibrate += OnHapticVibrate;
	}

	void OnDisable()
	{
		EventManager.OnHapticVibrate -= OnHapticVibrate;
	}

	void OnHapticVibrate(bool active)
	{
		UpdateSprite(active);
	}

	public Sprite haptic_on;
	public Sprite haptic_off;

	private Image image;


	void UpdateSprite(bool b)
	{
		if (image == null)
		{
			image = GetComponent<Image>();
		}

		image.sprite = b ? haptic_on : haptic_off;
	}

	void Start ()
	{
		UpdateSprite(GameplayController.HAPTIC);
	}

}
