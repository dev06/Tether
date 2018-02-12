using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelectUI : MonoBehaviour {

	public CanvasScaler scaler;
	private Vector3 lastMousePosition;
	private Transform tint;
	private float distanceVel;
	private int index;
	private float range;
	// Use this for initialization
	void Start ()
	{
		tint = transform.GetChild(0);
	}

	private float mouseDown;
	float vv;
	float delta;
	int holdIndex;
	bool isHolding;
	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			isHolding = true;
			mouseDown = Input.mousePosition.x;
			holdIndex = index;
		}

		if (Input.GetMouseButtonUp(0))
		{
			isHolding = false;
			delta = Input.mousePosition.x - mouseDown;

			float mag = Mathf.Abs(delta);
			if (mag < .20f * Screen.width) {
				range = -holdIndex;
				return;
			}
			if (delta < 0)
			{
				index++;
			}
			if (delta > 0)
			{
				index--;
			}

			index = Mathf.Clamp(index, 0, transform.childCount);

			range = -index;
		}

		if (isHolding)
		{
			// float diff = Mathf.Abs(mx - lastMousePosition.x);
			float mx = Input.mousePosition.x;
			range += ((mx - lastMousePosition.x)) / Screen.width;
			//	range = Mathf.Clamp(range, -transform.childCount, 0f);

		}

		Debug.LogError(scaler.referenceResolution);

		vv = Mathf.SmoothDamp(vv, range, ref distanceVel, Time.unscaledDeltaTime * 3f);

		transform.localPosition = new Vector2((vv * scaler.referenceResolution.x), 0);

		float x = Mathf.Clamp(transform.localPosition.x, -transform.childCount * scaler.referenceResolution.x, 0);
		transform.localPosition = new Vector2(x, 0);

		lastMousePosition = Input.mousePosition;
	}
}

