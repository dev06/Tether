using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelectUI : MonoBehaviour {

	public CanvasScaler scaler;
	private Vector3 lastMousePosition;
	private Vector3 mousePositionDown;
	private Vector3 mousePositionUp;
	private float distanceVel;
	private int index;
	private float range;
	private float size;
	private Menu menu;
	// Use this for initialization
	void Start ()
	{
		menu = FindObjectOfType<Menu>();
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
			holdIndex = index;
			mousePositionDown = Input.mousePosition;
			lastMousePosition = Input.mousePosition;

		}

		if (Input.GetMouseButtonUp(0))
		{
			mousePositionUp = Input.mousePosition;
			delta = mousePositionUp.x - mousePositionDown.x;
			isHolding = false;

			float mag = Mathf.Abs(delta);
			if (mag < 10)
			{

				transform.gameObject.SetActive(false);
				menu.StartGame();
			}
			if (mag < .20f * Screen.width) {
				range = -holdIndex * Screen.width;
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

			range = -index * Screen.width;


		}

		if (isHolding)
		{
			float mx = Input.mousePosition.x;
			range += ((mx - lastMousePosition.x));
		}


		vv = Mathf.SmoothDamp(vv, range, ref distanceVel, Time.unscaledDeltaTime * 2f);

		transform.localPosition = new Vector2((vv * 800f) / Screen.width, 0);

		float x = Mathf.Clamp(transform.localPosition.x, -transform.childCount * 800f, 0);
		transform.localPosition = new Vector2(x, 0);
		if (isHolding)
		{
			lastMousePosition = Input.mousePosition;
		}
	}

}





// if (Input.GetMouseButtonDown(0))
// {
// 	isHolding = true;
// 	mouseDown = Input.mousePosition.x;
// 	holdIndex = index;
// }

// if (Input.GetMouseButtonUp(0))
// {
// 	isHolding = false;
// 	delta = Input.mousePosition.x - mouseDown;

// 	float mag = Mathf.Abs(delta);
// 	if (mag < .20f * Screen.width) {
// 		range = -holdIndex * Screen.width;
// 		return;
// 	}
// 	if (delta < 0)
// 	{
// 		index++;
// 	}
// 	if (delta > 0)
// 	{
// 		index--;
// 	}

// 	index = Mathf.Clamp(index, 0, transform.childCount);

// 	range = -index * Screen.width;
// }

// if (isHolding)
// {
// 	// float diff = Mathf.Abs(mx - lastMousePosition.x);
// 	float mx = Input.mousePosition.x;
// 	range += ((mx - lastMousePosition.x));

// }

// //	Debug.LogError(range);

// vv = Mathf.SmoothDamp(vv, range, ref distanceVel, Time.unscaledDeltaTime * 3f);

// transform.localPosition = new Vector2((vv * 800f) / Screen.width, 0);

// float x = Mathf.Clamp(transform.localPosition.x, -transform.childCount * 800f, 0);
// transform.localPosition = new Vector2(x, 0);
// lastMousePosition = Input.mousePosition;