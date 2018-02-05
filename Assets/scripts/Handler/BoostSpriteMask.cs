using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSpriteMask : MonoBehaviour {

	private Camera camera;
	private CameraController cameraController; 
	private PlayerController player;
	private bool isInit;
	private SpriteRenderer renderer; 
	private float maskOffsetTimer; 
	private Transform circle; 

	public void Init()
	{
		camera = Camera.main;
		player = PlayerController.Instance;
		cameraController = camera.transform.GetComponent<CameraController>(); 
		renderer = GetComponent<SpriteRenderer>(); 
		circle = transform.GetChild(0).transform; 
		isInit = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (camera == null) { return; }
		if(player.activeBoost)
		{
			maskOffsetTimer+=Time.unscaledDeltaTime; 
			transform.position = new Vector3(camera.transform.position.x + Mathf.PingPong(maskOffsetTimer * 100f, .15f) - .075f, camera.transform.position.y, 0);
		}
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
		float hue = Mathf.PingPong(Time.time / 10f, 1.0f);
		float sat = 1f;
		float bright = .5f;
		return Color.HSVToRGB(hue, sat, bright);
	}
	float timer =1f;
	private IEnumerator IActivate()
	{	

		while (player.activeBoost)
		{
			timer += Time.unscaledDeltaTime * .3f ;
			timer = Mathf.Clamp(timer, 1f, timer);
			renderer.color = Color.white; 
			Camera.main.backgroundColor = GetColor(); 
			circle.transform.localPosition = Vector3.zero; 
			circle.localScale = new Vector3(transform.localScale.y, transform.localScale.x, 1); 
			transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(.2f, camera.orthographicSize * 2f), Time.unscaledDeltaTime * timer);
			float sy = camera.orthographicSize * 2f;
			transform.localScale = new Vector3(transform.localScale.x, sy, 1);
			yield return null; 
		}
		maskOffsetTimer = 0; 
		timer = 1f;
		//Camera.main.backgroundColor = Camera.main.GetComponent<CameraController>().defaultBackgroundColor;
		transform.gameObject.SetActive(false);
	}

}
