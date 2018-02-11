﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LogoParallex : MonoBehaviour {

	CameraController camera; 
	void Start ()
	{
		camera = Camera.main.GetComponent<CameraController>(); 
	}

	void Update ()
	{	
		if(outSide())
		{
			gameObject.SetActive(false); 
		}


		if(camera.isMoving)
		{
			float speed = camera.isMoving ? .5f : 0f; 
			transform.Translate(-Vector3.up * speed * Time.unscaledDeltaTime); 
		}
	}

	bool outSide()
	{
		Vector3 position = Camera.main.WorldToViewportPoint(transform.position); 
		return position.y < -1; 
	}

}
