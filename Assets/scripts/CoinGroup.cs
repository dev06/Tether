using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGroup : MonoBehaviour {


	public List<CoinController> CoinQueue = new List<CoinController>();

	ObjectSpawner spawner;
	PlayerController player;
	public Transform target;
	private Vector2 position;
	private bool b;

	void Start () {
	}

	public void Initialze()
	{
		InstantiateCoinObject(4);
		spawner = ObjectSpawner.Instance;
		player = PlayerController.Instance;

	}
	private void InstantiateCoinObject(int n)
	{
		for (int i = 0; i < n; i++)
		{
			GameObject clone = (GameObject)Instantiate(AppResources.Coin, new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
			clone.transform.SetParent(transform);
			CoinQueue.Add(clone.GetComponent<CoinController>());
			clone.SetActive(false);
		}
	}

	void Update()
	{

		if (IsOutsideOfBounds())
		{
			if (!b)
			{
				Toggle(false);
				b = true;
			}
		}


	}

	bool IsOutsideOfBounds()
	{
		Vector3 bounds = Camera.main.WorldToViewportPoint(transform.position);
		if (bounds.y < -.5f)
		{
			return true;
		}
		return false;
	}
	public void Toggle(bool b)
	{

		for (int i = 0; i < CoinQueue.Count; i++)
		{
			CoinQueue[i].transform.gameObject.SetActive(b);
		}
	}

	public bool HasRemainingCoins()
	{
		for (int i = 0; i < CoinQueue.Count; i++)
		{
			if (CoinQueue[i].gameObject.activeSelf)
			{
				return true;
			}
		}
		return false;
	}


	public void PositionCoins(Transform t)
	{
		// StopCoroutine("IPositionCoins");
		// StartCoroutine("IPositionCoins", spawner.nextBase.transform);
		Transform center = t;
		transform.position = center.position;
		float dt = (2f * Mathf.PI) / 4f;
		float radius = center.transform.localScale.x * .55f;

		for (int i = 0; i < CoinQueue.Count; i++)
		{
			CoinQueue[i].transform.gameObject.SetActive(true);
			float x = center.position.x + Mathf.Cos((float)i * dt) * radius;
			float y = center.position.y + Mathf.Sin((float)i * dt) * radius;
			Vector2 loc = new Vector2(x, y);
			CoinQueue[i].SetTargetLocation(loc);
		}

	}


	public void SetTarget(Transform current, Transform next)
	{
		Toggle(true);
		//SetCoins(current, next);
		PositionCoins(current);
		b = false;
	}


	public void SetCoins(Transform current, Transform next)
	{
		transform.position = next.position;
		RaycastHit2D hit = Physics2D.Raycast(current.position, -(current.position - next.position), 10, LayerMask.GetMask("Base") );
		RaycastHit2D hit2 = Physics2D.Raycast(next.position,    (current.position - next.position), 10, LayerMask.GetMask("Base"));

		Vector2 direction = hit2.point - hit.point;
		float centerSpacing = .8f;
		for (int i = 0; i < CoinQueue.Count; i++)
		{

			Vector2 offset = ((Vector2)hit.point + (Vector2)hit2.point) / 2f +  ((direction * centerSpacing) / 2f);
			CoinQueue[i].SetTargetLocation(((-(i + .5f) / CoinQueue.Count) * direction * centerSpacing + offset));
		}
	}
}
