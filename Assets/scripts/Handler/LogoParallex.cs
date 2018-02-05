using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoParallex : MonoBehaviour {

	PlayerController playerController;
	Vector3 startPos;  
	float offset; 

	void OnEnable()
	{
		EventManager.OnBaseHit+=OnBaseHit; 
	}

	void OnDisable()
	{
		EventManager.OnBaseHit-=OnBaseHit; 
	}

	void OnBaseHit()
	{
		offset-= playerController.activeBoost ? 10f : 5f;  

	}

	void Start () 
	{
		playerController = PlayerController.Instance; 
		startPos = transform.localPosition + Vector3.up * 10f; 
	}

	// Update is called once per frame
	void Update () 
	{
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, offset, 0),
		Time.deltaTime); 
		transform.localPosition = new Vector3(0, transform.localPosition.y, 0); 
	}

	bool Outside()
	{
		Vector3 position = Camera.main.WorldToViewportPoint(transform.position); 
		Debug.Log(position); 

		return position.y < .3; 
	}

}
