using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoostSpriteMask : MonoBehaviour {


	private PlayerController player;

	private Animation animation;

	private Camera camera;

	private Transform circle;

	private Image image;

	private bool isInit;

	public void Init()
	{

		camera = Camera.main;

		player = PlayerController.Instance;

		animation = GetComponent<Animation>();

		image = GetComponentInChildren<Image>();

		isInit = true;
	}

	void Update ()
	{
		if (camera == null) { return; }

		if (player.activeBoost)
		{

			boostShakeTimer += Time.unscaledDeltaTime;

			float shake = BoostShake() * boostShakeTimer * .8f;

			Vector3 calculatedShake = new Vector3(shake, 0, 0);

			calculatedShake.x = Mathf.Clamp(calculatedShake.x, -4f, 4f);

			transform.localPosition = calculatedShake;
		}
		else
		{
			boostShakeTimer = 0;
		}
	}

	private float boostShakeTimer;

	float BoostShake()
	{

		float offset = 20;

		return Mathf.PingPong(boostShakeTimer * 550f, offset) - (offset * .5f);
	}

	public void Activate()
	{
		if (!isInit)
		{
			Init();
		}

		image.enabled = true;

		animation.Play();
	}

	public bool isPlaying()
	{
		return animation.isPlaying;
	}
}
