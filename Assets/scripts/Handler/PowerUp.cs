using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	SpriteRenderer renderer;
	BoxCollider2D collider;

	void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<BoxCollider2D>();
	}

	void Update ()
	{
		renderer.enabled = IsInView();
		collider.enabled = renderer.enabled;
	}

	bool IsInView()
	{
		Vector3 port = Camera.main.WorldToViewportPoint(transform.position);

		if (port.y > 0 && port.y < 1)
		{
			return true;
		}

		return false;
	}
}
