using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSpriteMask : MonoBehaviour {

	private Camera camera;
	private PlayerController player;
	private bool isInit;
	private SpriteRenderer negative;
	void Start ()
	{
	}

	public void Init()
	{
		camera = Camera.main;
		player = PlayerController.Instance;
		negative = transform.GetChild(0).GetComponent<SpriteRenderer>();
		isInit = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (camera == null) return;
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
		float hue = Mathf.PingPong(Time.time / 25f, 1.0f);
		float sat = 1f;
		float bright = .5f;
		return Color.HSVToRGB(hue, sat, bright);
	}
	float timer = 2.5f;
	private IEnumerator IActivate()
	{

		while (player.activeBoost)
		{
			timer -= Time.deltaTime;
			timer = Mathf.Clamp(timer, 1.5f, timer);
			Color c = GetColor();
			transform.GetComponent<SpriteRenderer>().color = c;
			negative.color = new Color(1f - c.r, c.g, c.b, 1f);
			transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0f, camera.orthographicSize * 2f), Time.deltaTime * timer);
			float sy = camera.orthographicSize * 2f;
			transform.localScale = new Vector3(transform.localScale.x, sy, 1);

			yield return new WaitForSeconds(Time.deltaTime);
		}
		timer = 2.5f;
		camera.transform.GetComponent<CameraController>().SetGlow(.15f);
		transform.gameObject.SetActive(false);
	}

}
