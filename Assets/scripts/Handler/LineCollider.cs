using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollider : MonoBehaviour {

	public GameObject currentBase;

	public LineController line;
	Camera camera;

	void Start () {
		line = LineController.Instance;

		camera = Camera.main;
	}

	void Update () {
		if (IsOutsideOfBounds())
		{
			line.ResetLine();
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {

		//line.ResetLine();
		// Debug.Log(coll.gameObject == line.player.currentBase.transform.gameObject);
		// if (coll.gameObject == line.player.currentBase.transform.gameObject) { return; }
		// if (EventManager.OnBaseHit != null)
		// {
		// 	EventManager.OnBaseHit(coll.transform.GetComponent<BaseController>(), line.hitPoint);
		// }
	}

	bool IsOutsideOfBounds()
	{
		Vector3 bounds = camera.WorldToScreenPoint(transform.position);
		if (bounds.x > Screen.width || bounds.x < 0 || bounds.y > Screen.height || bounds.y < 0)
		{
			return true;
		}
		return false;
	}
}
