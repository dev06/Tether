using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class AudioSpecturm : MonoBehaviour {

	float[] spectrum = new float[256];
	public int id; 

	public Transform images; 
	void Start () {


		for(int i = 0;i < transform.childCount; i++)
		{
			RectTransform t = transform.GetChild(i).GetComponent<RectTransform>(); 
			float x = 800f / transform.childCount; 
			float y = t.sizeDelta.y; 
			t.sizeDelta = new Vector2(x, y); 
			t.anchoredPosition = new Vector2(i * x, 0); 
		}
	}

	// Update is called once per frame
	float vel; 
	float timer; 
	void Update () {


		AudioListener.GetSpectrumData( spectrum, 0, FFTWindow.Rectangular );

		for(int i = 0;i < transform.childCount; i++)
		{
			RectTransform t = transform.GetChild(i).GetComponent<RectTransform>(); 
			float raw =  (i + 1) * Mathf.Sin(spectrum[i + id]) * 1000f; 
			float target = Mathf.SmoothDamp(t.sizeDelta.y, raw, ref vel, Time.deltaTime * 1.5f); 
			float y = target; 

			t.sizeDelta = new Vector2(t.sizeDelta.x, y);
		}
	}
}
