using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoostSpriteMask : MonoBehaviour {

	private Camera camera;
	private CameraController cameraController;
	private PlayerController player;
	private bool isInit;
	private SpriteRenderer renderer;
	private float maskOffsetTimer;
	private Transform circle;
	private Image image;
	private Animation animation;

	public void Init()
	{
		camera = Camera.main;
		player = PlayerController.Instance;
		// cameraController = camera.transform.GetComponent<CameraController>();
		// renderer = GetComponent<SpriteRenderer>();
		// circle = transform.GetChild(0).transform;
		animation = GetComponent<Animation>();
		image = GetComponentInChildren<Image>();
		isInit = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (camera == null) { return; }
		if (player.activeBoost)
		{
			boostShakeTimer += Time.unscaledDeltaTime;
			float shake = BoostShake();
			// maskOffsetTimer += Time.unscaledDeltaTime;
			// float offset = .2f;
			// float horizontalOffset = Mathf.PingPong(maskOffsetTimer * 100f, offset) - (offset * .5f);
			transform.localPosition = new Vector3(shake, 0, 0);
		}
		else
		{
			boostShakeTimer = 0;
		}
		// transform.localScale = new Vector3(transform.localScale.x, camera.orthographicSize * 2f, 1f);
	}

	private float boostShakeTimer;

	float BoostShake()
	{
		float offset = 20f;
		return Mathf.PingPong(boostShakeTimer * 550f, offset) - (offset * .5f);
	}

	public void Activate()
	{
		// transform.gameObject.SetActive(true);
		if (!isInit)
		{
			Init();
		}
		// transform.localScale = new Vector3(camera.orthographicSize, camera.orthographicSize * 2f, 1);
		// transform.position = camera.transform.position;

		image.enabled = true;
		animation.Play();


		//	StopCoroutine("IActivate");
		//	StartCoroutine("IActivate");
	}

	public bool isPlaying()
	{
		return animation.isPlaying;
	}
	// Color GetColor()
	// {
	// 	float hue = Mathf.PingPong(Time.time / 10f, 1.0f);
	// 	float sat = 1f;
	// 	float bright = 1f;
	// 	return Color.HSVToRGB(hue, sat, bright);
	// }
	// float timer = 1f;
	// private IEnumerator IActivate()
	// {

	// 	while (player.activeBoost)
	// 	{
	// 		//timer += Time.unscaledDeltaTime * .3f ;
	// 		timer = Mathf.Clamp(timer, 1f, timer);
	// 		renderer.color = Color.white;
	// 		//Camera.main.backgroundColor = GetColor();
	// 		circle.transform.localPosition = Vector3.zero;
	// 		circle.localScale = new Vector3(transform.localScale.y, transform.localScale.x, 1);
	// 		transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(.3f, camera.orthographicSize * 2f), Time.unscaledDeltaTime * 2f);
	// 		float sy = camera.orthographicSize * 2f;
	// 		transform.localScale = new Vector3(transform.localScale.x, sy, 1);
	// 		yield return null;
	// 	}
	// 	maskOffsetTimer = 0;
	// 	timer = 1f;
	// 	transform.gameObject.SetActive(false);
	// }

}
