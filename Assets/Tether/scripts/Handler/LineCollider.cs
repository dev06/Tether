using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollider : MonoBehaviour {


	// LineController lineController; 
	// BoxCollider2D collider; 

	// void Start()
	// {
	// 	lineController = GetComponentInParent<LineController>(); 
	// 	collider = GetComponent<BoxCollider2D>();
	// 	collider.size = new Vector2(1,1); 
	// }


	// void Update()
	// {
	// //	transform.position = lineController.line.GetPosition(1); 
	// }

	// void OnTriggerEnter2D(Collider2D col)
	// {
	// 	// Camera.main.GetComponent<CameraController>().freezeCamera = true;

	// 	// // lineController.hitWall = true; 



	// 	// // object[] objs = new object[3] {null, new Vector2(transform.position.x, transform.position.y), col.transform.gameObject};

	// 	// // lineController.SetEndLinePosition(transform.position); 

	// 	// // lineController.AttachM(objs); 

	// 	// Time.timeScale = .1f;

	// 	// Time.fixedDeltaTime = Time.timeScale * .02f;

	// 	// StopCoroutine("LerpTimeToNormal");

	// 	// StartCoroutine("LerpTimeToNormal");


	
	// }

	// IEnumerator LerpTimeToNormal()
	// {
	// 	float timeScaleVel = 0;

	// 	while (Time.timeScale < 1)
	// 	{
	// 		Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 1f, ref timeScaleVel, Time.unscaledDeltaTime * 6f);

	// 		Time.fixedDeltaTime = Time.timeScale * .02f;

	// 		if(Time.timeScale > .5f)
	// 		{

	// 		}



	// 		yield return null;
	// 	}



	// }




}
