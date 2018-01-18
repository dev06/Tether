using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CoinController : MonoBehaviour {


	private Vector2 targetLocation;

	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, targetLocation, Time.deltaTime * 20.0f);
	}

	public void SetTargetLocation(Vector2 location)
	{
		this.targetLocation = location;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			//gameObject.SetActive(false);
		}
	}
}