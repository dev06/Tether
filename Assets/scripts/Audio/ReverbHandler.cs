using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReverbHandler : MonoBehaviour {


	void OnEnable()
	{
		EventManager.OnReverb += OnReverb;
	}

	void OnDisable()
	{
		EventManager.OnReverb -= OnReverb;
	}

	void OnReverb(bool active)
	{
		UpdateSprite(active);
	}


	public Sprite reverb_on;
	public Sprite reverb_off;

	private Image image;


	void UpdateSprite(bool b)
	{
		if (image == null)
		{
			image = GetComponent<Image>();
		}

		image.sprite = b ? reverb_on : reverb_off;
	}

	void Start ()
	{
		UpdateSprite(AudioController.ReverbOn);
	}
}
