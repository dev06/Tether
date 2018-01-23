using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

	public static ObjectSpawner Instance;


	public List<BaseController> BaseQueue = new List<BaseController>();

	public List<CoinController> CoinQueue = new List<CoinController>();

	public List<CoinGroup> CoinGroupQueue = new List<CoinGroup>();

	public int boom_index = 0;

	public int smoke_index = 0;

	public int powerup_index = 0;

	public Transform Particle_Boom;
	public Transform Particle_Smoke;

	public Transform obj_powerup;
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


		PositionBases();
	}

	void Update()
	{
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
			BaseController controller = clone.GetComponent<BaseController>();
			BaseQueue.Add(controller);
			controller.Initialize();
		}
	}

	private void InstantiateCoinGroup(int n)
	{
		for (int i = 0; i < n; i++)
		{
			GameObject clone = (GameObject)Instantiate(AppResources.CoinGroup, new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
			clone.transform.SetParent(obj_coingroup);
			CoinGroupQueue.Add(clone.GetComponent<CoinGroup>());
			clone.transform.GetComponent<CoinGroup>().Initialze();
		}
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
				float yRange = Random.Range(3f, 4f);
				BaseQueue[i].transform.position = previousBase.transform.position + new Vector3(xRange, yRange, 0);
				float scale = Random.Range(1f, 1.5f);
				BaseQueue[i].transform.localScale = new Vector3(scale, scale, scale);
			}
		}
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

	public void PlaceCoin(int i, int j )
	{
		BaseController b2 = GetBase(j);
		BaseController b1 = GetBase(i);

		CoinGroup grp = FetchNextCoinGroup();
		grp.SetTarget(b1.transform, b2.transform);
	}

	public BaseController GetBase(int n)
	{
		BaseController toRet = null;
		for (int i = 0; i < BaseQueue.Count; i++)
		{
			if (BaseQueue[i] != null)
			{
				if (i + n > BaseQueue.Count - 1)
				{
					toRet = BaseQueue[0];
					break;
				} else
				{
					toRet = BaseQueue[i + n];
					break;
				}
			}
		}

		return toRet;
	}

	public  BaseController FetchNextBase()
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

		this.nextBase = nextBase;
		nextBase.transform.gameObject.SetActive(true);
		return nextBase;
	}

	public void SpawnParticle(ParticleType type, Vector3 position, Color c)
	{
		switch (type)
		{
			case ParticleType.BOOM:
			{
				Particle_Boom.transform.GetChild(boom_index).GetComponent<Particle>().Play(position, c);

				boom_index++;
				if (boom_index > Particle_Boom.childCount - 1)
				{
					boom_index = 0;
				}
				break;
			}
		}
	}

	public void SpawnParticle(ParticleType type, Vector3 position)
	{
		switch (type)
		{
			case ParticleType.BOOM:
			{
				Particle_Boom.transform.GetChild(boom_index).GetComponent<Particle>().Play(position);

				boom_index++;
				if (boom_index > Particle_Boom.childCount - 1)
				{
					boom_index = 0;
				}
				break;
			}

			case ParticleType.SMOKE:
			{
				Particle_Smoke.transform.GetChild(smoke_index).GetComponent<Particle>().Play(position);

				smoke_index++;
				if (smoke_index > Particle_Smoke.childCount - 1)
				{
					smoke_index = 0;
				}
				break;
			}
		}
	}

	public void SpawnPowerup(Vector3 position)
	{
		GameObject obj = obj_powerup.transform.GetChild(powerup_index).transform.gameObject;
		obj.SetActive(true);
		obj.transform.position = position;
		powerup_index++;
		if (powerup_index > obj_powerup.childCount - 1)
		{
			powerup_index = 0;
		}
	}
}

public enum ParticleType
{
	BOOM,
	EFFECT,
	SMOKE,

}
