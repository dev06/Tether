using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSpriteMask : MonoBehaviour {

	private Camera camera;
	private CameraController cameraController; 
	private PlayerController player;
	private bool isInit;
	private SpriteRenderer negative;
	private SpriteRenderer renderer; 
	private float saturation; 

	public void Init()
	{
		camera = Camera.main;
		player = PlayerController.Instance;
		cameraController = camera.transform.GetComponent<CameraController>(); 
		renderer = GetComponent<SpriteRenderer>(); 
		negative = transform.GetChild(0).GetComponent<SpriteRenderer>();

		isInit = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (camera == null) { return; }
		transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, 0);
		transform.localScale = new Vector3(transform.localScale.x, camera.orthographicSize * 2f, 1f);
	}

	public void Activate()
	{
		transform.gameObject.SetActive(true);
		if (!isInit)
		{
			Init();
		}
		transform.localScale = new Vector3(camera.orthographicSize, camera.orthographicSize * 2f, 1);
		transform.position = camera.transform.position;


		StopCoroutine("IActivate");
		StartCoroutine("IActivate");
	}
	Color GetColor()
	{
		float hue = Mathf.PingPong(Time.time / 20f, 1.0f);
		float sat = 1f;
		float bright = .5f;
		return Color.HSVToRGB(hue, sat, bright);
	}
	float timer =2.5f;
	private IEnumerator IActivate()
	{	

		while (player.activeBoost)
		{
			timer -= Time.deltaTime;
			timer = Mathf.Clamp(timer, 1.7f, timer);
			renderer.color = Color.white; 

			negative.color = GetColor();  
			Camera.main.backgroundColor = new Color(negative.color.r, 1f- negative.color.g, 1f - negative.color.b, 1f); 
			transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0f, camera.orthographicSize * 2f), Time.deltaTime * timer);
			float sy = camera.orthographicSize * 2f;
			transform.localScale = new Vector3(transform.localScale.x, sy, 1);
			yield return null; 
		}
		timer = 2.5f;
		cameraController.SetGlow(.15f);
		Camera.main.backgroundColor = Camera.main.GetComponent<CameraController>().defaultBackgroundColor;
		transform.gameObject.SetActive(false);
	}

}
