using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {

	public PlayerController player;
	float depletionRate  = .08f;

	public SpriteRenderer spriteRenderer;

	public Vector3 targetLocation;
	public Vector3 targetScale;
	public bool hadPlayer;

	public bool shouldRotate = true;
	public static float DIRECTION = -1f;
	void Start()
	{

	}

	void Update()
	{
		if (targetLocation != null && hadPlayer)
		{
			transform.position = Vector3.Lerp(transform.position, targetLocation, Time.deltaTime / transform.localScale.x);
			transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime / transform.localScale.x);
		}
		// Vector3 position = Camera.main.WorldToViewportPoint(transform.position);
		// Debug.Log(position);
		if (player != null)
		{
			if (shouldRotate) {
				transform.Rotate(Vector3.forward, Time.deltaTime * 160f * DIRECTION);
			}
			if (transform.localScale.magnitude > 0) {
				//transform.localScale -= new Vector3(Time.deltaTime * depletionRate, Time.deltaTime * depletionRate, Time.deltaTime * depletionRate);
			}
		} else
		{
			gameObject.layer = 8;
		}

		if (IsOutsideOfBounds())
		{
			//transform.position = new Vector3(transform.position.x, 4f, 0);
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(transform.position.x, Screen.height, 0));
		}
	}

	bool IsOutsideOfBounds()
	{
		Vector3 bounds = Camera.main.WorldToScreenPoint(transform.position);
		if (bounds.x > Screen.width || bounds.x < 0 || bounds.y > Screen.height || bounds.y < 0)
		{
			return true;
		}
		return false;
	}
}

