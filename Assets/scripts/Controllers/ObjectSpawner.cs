using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

	public static ObjectSpawner Instance;


	public List<BaseController> BaseQueue = new List<BaseController>();

	public int boom_index = 0;

	public int smoke_index = 0;

	public int powerup_index = 0;

	public int  onLineHitBase_index = 0;

	public Transform Particle_Boom;

	public Transform Particle_Smoke;

	public Transform Partilce_OnLineHitBase;

	public Transform obj_powerup;

	public Transform obj_bases;

	private Transform obj_coingroup;

	private LevelController levelController;

	private LineController lineController;

	private AudioController audioController;

	private bool isInit;

	public BaseController nextBase;

	PlayerController player;

	void OnEnable()
	{
	}

	void OnDisable()
	{
	}



	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			DestroyImmediate(gameObject);
		}

	}


	public void Init()
	{

		player = PlayerController.Instance;

		lineController = LineController.Instance;

		levelController = LevelController.Instance;

		audioController = AudioController.Instance;

		obj_bases = GameObject.FindWithTag("Objects/bases").transform;

		InstantiateBaseObject(3, Vector2.up * 10);

		PositionBases();

		player.Init();

		lineController.Init();

		if (audioController != null)
		{
			audioController.Init();
		}

		isInit = true;
	}

	void Update()
	{
		if (!isInit) { return ; }

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
			//clone.GetComponent<SpriteRenderer>().color = levelController.GetLevelConstants().BaseColor;
			controller.Initialize();
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
				float xRange = 0;
				float yRange = Random.Range(6f, 8f);
				BaseQueue[i].transform.position = previousBase.transform.position + new Vector3(xRange, yRange, 0);
			}

			float scale = BaseController.DEFAULT_MAX_SCALE;
			BaseQueue[i].transform.localScale = new Vector3(scale, scale, scale);
			BaseQueue[i].transform.gameObject.SetActive(false);
		}
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
				}
				else
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
		BaseController currentBase = null;
		for (int i = 0; i < BaseQueue.Count; i++)
		{
			if (BaseQueue[i].player != null)
			{
				currentBase = BaseQueue[i];
				if (i + 1 > BaseQueue.Count - 1)
				{
					nextBase = BaseQueue[0];
				}
				else
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
		currentBase.transform.gameObject.SetActive(true);
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

			case ParticleType.ONLINEHITBASE:
			{
				Partilce_OnLineHitBase.transform.GetChild(onLineHitBase_index).GetComponent<Particle>().Play(position, c);

				onLineHitBase_index++;
				if (onLineHitBase_index > Partilce_OnLineHitBase.childCount - 1)
				{
					onLineHitBase_index = 0;
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
	ONLINEHITBASE,

}
