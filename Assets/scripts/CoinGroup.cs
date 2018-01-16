using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGroup : MonoBehaviour {


	public List<CoinController> CoinQueue = new List<CoinController>();

	ObjectSpawner spawner;
	public Transform target;

	void Start () {
		InstantiateCoinObject(10);

		spawner = ObjectSpawner.Instance;

	}
	private void InstantiateCoinObject(int n)
	{
		for (int i = 0; i < n; i++)
		{
			GameObject clone = (GameObject)Instantiate(AppResources.Coin, new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
			clone.transform.SetParent(transform);
			clone.SetActive(false);
			CoinQueue.Add(clone.GetComponent<CoinController>());
		}
	}

	void Update () {

	}


	public void Toggle(bool b)
	{
		foreach (CoinController c in CoinQueue)
		{
			c.gameObject.SetActive(b);
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
		float dt = (2f * Mathf.PI) / 10f;
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

	// public IEnumerator IPositionCoins(Transform center)
	// {
	// 	// int i = 0;
	// 	// float dt = (2f * Mathf.PI) / 10f;
	// 	// float radius = center.transform.localScale.x * .55f;

	// 	// while (i < CoinQueue.Count)
	// 	// {
	// 	// 	CoinQueue[i].transform.gameObject.SetActive(true);
	// 	// 	float x = center.position.x + Mathf.Cos((float)i * dt) * radius;
	// 	// 	float y = center.position.y + Mathf.Sin((float)i * dt) * radius;
	// 	// 	Vector2 loc = new Vector2(x, y);
	// 	// 	CoinQueue[i].SetTargetLocation(loc);
	// 	// 	i++;
	// 	// 	yield return null;
	// 	// }
	// }


}
