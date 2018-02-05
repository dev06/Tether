using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	SpriteRenderer renderer; 

	void Start()
	{
		renderer = GetComponent<SpriteRenderer>(); 
	}
	
	void Update () 
	{
		renderer.enabled = IsInView(); 
	}

	bool IsInView()
	{
		Vector3 port = Camera.main.WorldToViewportPoint(transform.position); 
		
		if(port.y > 0 && port.y < 1)
		{
			return true; 
		}

		return false; 
	}
}
