using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MuteHandler : MonoBehaviour {


	public Sprite mute;
	private Sprite unmute;
	private Image image;

	void OnEnable()
	{
		EventManager.OnMute += OnMute;
	}
	void OnDisable()
	{
		EventManager.OnMute -= OnMute;
	}
	void Start ()
	{
		image = GetComponent<Image>();
		unmute = image.sprite;
		UpdateSprite(AudioController.Mute);
	}

	void OnMute(bool b)
	{
		UpdateSprite(b);
	}

	private void UpdateSprite(bool b)
	{
		Sprite s = b ? mute : unmute;
		image.sprite = s;
	}

	void Update ()
	{

	}
}
