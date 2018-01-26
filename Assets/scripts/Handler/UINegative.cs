using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UINegative : MonoBehaviour {

	public Image child;
	Image instance;
	public Color instanceColor;

	void Start ()
	{
		instanceColor = transform.GetComponent<Image>().color;
		instance = transform.GetComponent<Image>();
	}

	// Update is called once per frame
	void Update ()
	{
		instance.color = CreateNormal();
		child.color = CreateNegative(instance.color);
	}

	Color CreateNormal()
	{
		float multiplier = 1.2f;
		float hue = Mathf.PingPong(Time.time  * multiplier, 1.0f);
		float sat = 1f;
		float bright = .2f;
		return Color.HSVToRGB(hue, sat, bright);
	}

	Color CreateNegative(Color color)
	{
		return new Color(1f - color.r, color.g, color.b, 1f);
	}
}
