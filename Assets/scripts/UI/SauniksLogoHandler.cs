using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class SauniksLogoHandler : MonoBehaviour {

	LevelController levelController; 

	public Transform[] logos; 

	void OnEnable()
	{
		EventManager.OnLevelChange+=OnLevelChange; 
	}

	void OnDisable()
	{
		EventManager.OnLevelChange-=OnLevelChange; 
	}

	void OnLevelChange(Level l)
	{
		ActivateLogo(l); 
	}


	void Start ()
	{
		//levelController = LevelController.Instance; 
	}

	void ActivateLogo(Level l)
	{
		for(int i =0 ;i < logos.Length; i++)
		{
			logos[i].gameObject.SetActive(false); 
		}

		switch(l)
		{
			case Level.LEVEL1: 
			{
				logos[0].gameObject.SetActive(true); 
				break; 
			}

			case Level.LEVEL2: 
			{
				logos[1].gameObject.SetActive(true); 
				break; 
			}			
		}
	}
}
