using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public static PlayerController Instance;
	public BaseController currentBase;

	private LineController line;
	private int direction = -1;
	private Vector3 targetLocation = Vector3.zero;
	private float defaultScale;
	private float scale;
	private float distaceToKeep;
	private Vector3 targetLocalPosition;
	ObjectSpawner objectSpawner;
	public GameObject effect;
	public ParticleSystem[] system;
	private SpriteRenderer renderer;
	private CameraController cameraController;
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		} else
		{
			DestroyImmediate(gameObject);
		}



		targetLocation = transform.position;
		targetLocalPosition = transform.localPosition;

	}

	void OnEnable()
	{
		EventManager.OnBaseHit += OnBaseHit;
	}

	void OnDisable() {
		EventManager.OnBaseHit -= OnBaseHit;

	}

	Vector3 targetPos;
	void Start ()
	{
		line = transform.GetChild(0).GetComponent<LineController>();
		objectSpawner = ObjectSpawner.Instance;

		if (currentBase != null)
		{
			currentBase.player = this;
		}

		defaultScale = transform.localScale.x;
		targetPos  = transform.position;
		targetLocation = transform.localPosition;
		cameraController = Camera.main.GetComponent<CameraController>();
		renderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
		SetCurrentBase();

	}


	float rotatingDir;
	[Range(.5f, 5f)]
	public float aa = 0;
	public float angle = 0;
	float timer = 0;
	float boostTimer = 0;
	public bool activeBoost;
	public BoostSpriteMask bsm;
	void Update ()
	{
		transform.right = transform.position - currentBase.transform.position;

		scale = (defaultScale / currentBase.transform.localScale.x);
		transform.localScale = new Vector3(scale, scale, scale);

		if (Input.GetKeyDown(KeyCode.L))
		{
			activeBoost = true;
			cameraController.StartVortex();
			bsm.Activate();

		}
		if (boostTimer > 4f)
		{
			boostTimer = 0;
			BaseController.VELOCITY_SCALE = 1f;
			activeBoost = false;
			boostTimer = 0;
			spikes.GetComponent<Particle>().Stop();
			spikes.gameObject.SetActive(false);
			cameraController.SetGlow(.14f);


		}
		if (activeBoost)
		{
			UpdateBoost();
		}
	}

	float timeVel;
	void UpdateBoost()
	{
		if (!activeBoost) {
			return;
		}
		boostTimer += Time.deltaTime;
		BaseController.VELOCITY_SCALE = 0f;
		timer += Time.deltaTime;
		transform.position = objectSpawner.nextBase.transform.position;
		line.Shoot();
		Round();
		Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 1.5f, ref timeVel, Time.unscaledDeltaTime * 100f);
		Time.fixedDeltaTime = Time.timeScale * .02f;
		spikes.gameObject.SetActive(true);
		spikes.position = Camera.main.ViewportToWorldPoint(new Vector2(.5f, 0));
		spikes.GetComponent<Particle>().Play();
		float shake = Random.Range(3.0f, 3.7f);
		float shrinkFactor = Random.Range(8.0f, 8.5f);
		Camera.main.transform.GetComponent<CameraController>().Jitter(shake, shrinkFactor);
	}

	public void SetCurrentBase()
	{
		BaseController b = objectSpawner.obj_bases.transform.GetChild(0).transform.GetComponent<BaseController>();
		currentBase = b;
		SetTargetBase(currentBase);
		Round();
	}

	public void Round()
	{
		angle = (int)CalculateAngle(transform.position, currentBase.transform.position);
		float y = aa * Mathf.Sin(angle * Mathf.Deg2Rad);
		float xx = aa * Mathf.Cos(angle * Mathf.Deg2Rad);
		Vector3 pos = new Vector3(xx, y, 0);
		transform.localPosition  = pos;
	}

	public  float CalculateAngle(Vector3 from, Vector3 to)
	{
		return Quaternion.FromToRotation(-currentBase.transform.right, to - from).eulerAngles.z;
	}

	void OnBaseHit(BaseController hitBase, Vector2 hitPoint)
	{
	}

	public void SetTargetLocation(Vector2 targetLocation)
	{
		this.targetLocation = targetLocation;
	}

	public void SetLocation(Vector2 location)
	{
		transform.position = location;
	}

	public void SetTargetBase(BaseController hitBase)
	{
		currentBase.player = null;
		currentBase.transform.gameObject.layer = 8;
		currentBase = hitBase;
		transform.SetParent(currentBase.transform);
		currentBase.player = this;
		currentBase.transform.gameObject.layer = 9;
		//BaseController.DIRECTION *= GetDirection();
	}


	public Transform spikes;
	private Color scheme = Color.black;
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject == currentBase.transform.gameObject)
		{
			if (!activeBoost)
			{
				float shake = Random.Range(2.0f, 2.3f);
				float shrinkFactor = Random.Range(6.0f, 6.5f);
				Camera.main.transform.GetComponent<CameraController>().Jitter(shake, shrinkFactor);
			}
			if (scheme.r == 0f)
			{
				scheme = Color.white;
			} else if (scheme.r == 1f)
			{
				scheme = Color.black;
			}
			objectSpawner.SpawnParticle(ParticleType.BOOM, currentBase.transform.position, scheme);

			InvertColors();




		}

	}

	void InvertColors()
	{
		Color current = renderer.color;
		Color invert = scheme;
		system[0].startColor = invert;
		system[1].startColor = invert;
		currentBase.SetColor(invert);
		line.line.SetColors(scheme, scheme);
		if (invert.r == 1)
		{
			cameraController.SetGlow(.118f);
		}
		renderer.color = invert;

		if (renderer.color.r == 0)
		{
			objectSpawner.SpawnParticle(ParticleType.SMOKE, currentBase.transform.position);

		} else
		{
			currentBase.SpawnRing(scheme);

		}
	}

	public void SetTargetLocalPosition(Vector3 position)
	{
		this.targetLocalPosition = position;
	}


	private int GetDirection()
	{
		int value = Random.Range(-1, 1);
		if (value == 0) { return 1; }
		return value;
	}


}
