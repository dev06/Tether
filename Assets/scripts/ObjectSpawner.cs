using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

	public static ObjectSpawner Instance;


	public List<BaseController> BaseQueue = new List<BaseController>();

	public List<CoinController> CoinQueue = new List<CoinController>();

	public List<CoinGroup> CoinGroupQueue = new List<CoinGroup>();


	public Transform obj_bases;
	private Transform obj_coingroup;

	public BaseController nextBase;

	PlayerController player;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		} else
		{
			DestroyImmediate(gameObject);
		}
	}

	void Start ()
	{
		player = PlayerController.Instance;

		obj_bases = GameObject.FindWithTag("Objects/bases").transform;
		obj_coingroup = GameObject.FindWithTag("Objects/coingroup").transform;

		InstantiateBaseObject(5, Vector2.up * 4);
		InstantiateCoinGroup(3);
		//PositionNextBase();

		PositionBases();
	}

	void Update()
	{
		//CalculateDirection
		if (Input.GetKeyDown(KeyCode.L))
		{

		}

		FetchNextBase();


	}


	private void InstantiateBaseObject(int n, Vector2 initialPostion)
	{
		for (int i = 0; i < n; i++)
		{
			GameObject clone = (GameObject)Instantiate(AppResources.Base, initialPostion, Quaternion.identity) as GameObject;
			clone.transform.SetParent(obj_bases);
			clone.name = "Base_" + i;
			clone.SetActive(false);
			BaseQueue.Add(clone.GetComponent<BaseController>());
		}
	}

	// private void InstantiateCoinObject(int n)
	// {
	// 	for (int i = 0; i < n; i++)
	// 	{
	// 		GameObject clone = (GameObject)Instantiate(AppResources.Coin, new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
	// 		clone.transform.SetParent(obj_coins);
	// 		//clone.SetActive(false);
	// 		CoinQueue.Add(clone.GetComponent<CoinController>());
	// 	}
	// }

	private void InstantiateCoinGroup(int n)
	{
		for (int i = 0; i < n; i++)
		{
			GameObject clone = (GameObject)Instantiate(AppResources.CoinGroup, new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
			clone.transform.SetParent(obj_coingroup);
			CoinGroupQueue.Add(clone.GetComponent<CoinGroup>());
		}
	}

	private BaseController FetchNextBase()
	{
		BaseController nextBase = null;
		for (int i = 0; i < BaseQueue.Count; i++)
		{
			if (BaseQueue[i].player != null)
			{
				if (i + 1 > BaseQueue.Count - 1)
				{
					nextBase = BaseQueue[0];
				} else
				{
					nextBase = BaseQueue[i + 1];
				}
			}
		}

		if (nextBase == null)
		{
			nextBase = BaseQueue[0];
		}

		nextBase.transform.gameObject.SetActive(true);

		//nextBase = obj_bases.GetChild(0).transform.GetComponent<BaseController>();
		this.nextBase = nextBase;
		return nextBase;
	}

	public void CalculateDirection()
	{
		//Debug.DrawRay(nextBase.transform.position, -(nextBase.transform.position - player.currentBase.transform.position));
		RaycastHit2D hit = Physics2D.Raycast(nextBase.transform.position, -(nextBase.transform.position - player.currentBase.transform.position), 10, LayerMask.GetMask("CurrentBase"));
		//Debug.Log(hit.collider);
		BaseController hitBase = hit.transform.GetComponent<BaseController>();
		hitBase.targetPoint = hit.point;
		BaseController.DIRECTION = GetDirection(CalculateAngle(hit.point, player.transform.position));

	}

	public  float CalculateAngle(Vector3 from, Vector3 to)
	{
		return Quaternion.FromToRotation(from, to).eulerAngles.z;
	}

	public void PositionBases()
	{
		for (int i = 0; i < BaseQueue.Count; i++)
		{
			BaseQueue[i].transform.gameObject.SetActive(true);


			BaseController currentBase = BaseQueue[i];
			BaseController previousBase = null;
			if (i >= 1)
			{
				previousBase = BaseQueue[i - 1];
			}

			if (previousBase != null)
			{
				float xRange = Random.Range(-1.15f, 2.15f);
				float yRange = Random.Range(3f, 5f);
				BaseQueue[i].transform.position = previousBase.transform.position + new Vector3(xRange, yRange, 0);
			}


		}

	}


	public void PositionNextBase()
	{
		BaseController nextBase = FetchNextBase();
		BaseController currentBase = PlayerController.Instance.currentBase;
		float xRange = Random.Range(-1.15f, 2.15f);
		float yRange = Random.Range(3f, 5f);
		Vector3 basePosition = currentBase.transform.position + new Vector3(xRange, yRange, currentBase.transform.position.z);
		float distance = Vector3.Distance(basePosition, currentBase.transform.position);

		float scale = distance * Random.Range(.25f, .4f);
		nextBase.transform.localScale = new Vector3(scale, scale, 1);

		nextBase.transform.position = basePosition;
		nextBase.transform.rotation = Quaternion.Euler(Vector2.zero);
		float minBound = 5f;
		float maxBound = 6f;
		currentBase.SetVelocity(Random.Range(minBound, maxBound));

		CoinGroup grp = FetchNextCoinGroup();

		grp.SetTarget(player.currentBase.transform, nextBase.transform);

	}


	public CoinGroup FetchNextCoinGroup()
	{
		for (int i = 0; i < CoinGroupQueue.Count; i++)
		{
			CoinGroup grp = CoinGroupQueue[i];
			if (grp.HasRemainingCoins() == false)
			{
				return grp;
			}
		}
		return CoinGroupQueue[0];
	}

	int GetDirection(float v)
	{
		return (v > 180) ? 1 : -1;
	}
}
