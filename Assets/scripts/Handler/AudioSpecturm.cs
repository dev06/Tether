using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpecturm : MonoBehaviour {

	float[] spectrum = new float[256];
	int lineAmount = 15;
	public LineRenderer Line;
	void Start () {
		Line = transform.GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update () {
		AudioListener.GetSpectrumData( spectrum, 0, FFTWindow.Rectangular );

		for (int i = 0; i < lineAmount; i++ )
		{
			Vector3 pos = new Vector3(i - lineAmount / 4.0f,  Mathf.Sin(spectrum[i]) * lineAmount / 2.0f - 49 , 0);

			Line.SetPosition(i, pos);
			Color one = new Color(1f, 1f, 1f, 1f);
			Color two = new Color(spectrum[i] * 255f, spectrum[i] * 255f, 1f, 1f);

			Line.SetColors(one, two);
		}

	}
}
