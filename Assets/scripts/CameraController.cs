using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	PlayerController player;
	private BaseController[] bases;
	void Start ()
	{
		player = PlayerController.Instance;
		bases = GameObject.FindObjectsOfType<BaseController>();
		Camera.main.orthographicSize = 4f * Screen.height / Screen.width * 0.5f;
	}

	// Update is called once per frame
	void Update ()
	{
		transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up + new Vector3(0, 0, -10f), Time.deltaTime / 2.0f);

	}

	Vector3 GetAveragePosition()
	{
		float xSum = 0;
		float ySum = 0;
		Vector3 average = Vector3.zero;
		for (int i = 0 ; i < bases.Length; i++)
		{
			xSum += bases[i].transform.position.x;
			ySum += bases[i].transform.position.y;
		}

		xSum /= bases.Length;
		ySum /= bases.Length;

		average = new Vector3(xSum, ySum, -10);
		return average;
	}
}
