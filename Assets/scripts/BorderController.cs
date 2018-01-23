using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour {

	Camera camera;
	Vector3 position;
	float borderThickness = .2f;

	public enum BorderType
	{
		LEFT,
		RIGHT,
		TOP,
		BOTTOM,
	}

	public BorderType type;

	void Start ()
	{
		camera = Camera.main;
	}


	void Update ()
	{
		SetPosition(type);
	}

	void SetPosition(BorderType type)
	{
		switch (type)
		{
			case BorderType.LEFT:
			{
				position = Camera.main.ViewportToWorldPoint(new Vector3(0, .5f, 0));
				position.z = 0;
				transform.localScale = new Vector3(borderThickness + .1f, camera.orthographicSize * 2, 0);
				transform.position = position;
				break;
			}
			case BorderType.RIGHT:
			{
				position = Camera.main.ViewportToWorldPoint(new Vector3(1, .5f, 0));
				position.z = 0;
				transform.localScale = new Vector3(borderThickness + .1f, camera.orthographicSize * 2, 0);
				transform.position = position;
				break;
			}

			case BorderType.TOP:
			{
				position = Camera.main.ViewportToWorldPoint(new Vector3(.5f, 1, 0));
				position.z = 0;
				transform.localScale = new Vector3(camera.orthographicSize, borderThickness, 0);
				transform.position = position;
				break;
			}
			case BorderType.BOTTOM:
			{
				position = Camera.main.ViewportToWorldPoint(new Vector3(.5f, 0, 0));
				position.z = 0;
				transform.localScale = new Vector3(camera.orthographicSize, borderThickness, 0);
				transform.position = position;
				break;
			}
		}
	}
}
