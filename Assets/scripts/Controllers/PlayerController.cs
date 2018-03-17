using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{


	public static PlayerController Instance;

	public bool activeBoost;

	public AudioSource sfx;

	public BoostSpriteMask bsm;

	public BaseController currentBase;

	public ParticleSystem[] particleSystems;

	public Particle slowmotionParticle, onLineHitBase;

	public Transform spikes;

	public ParticleSystem fan, blackBase, boostPrism;

	private float base_hit_glow = .13f;

	private LineController line;

	private int direction = -1;

	private Vector3 targetLocation = Vector3.zero;

	private float defaultScale;

	private float scale;

	private float timeVel;

	private float distaceToKeep;

	private Vector3 targetLocalPosition;

	private ObjectSpawner objectSpawner;

	private SpriteRenderer renderer;

	private CameraController cameraController;

	private int boostScoreAddition = 25;

	private int idealScore = 0;

	private Color scheme = Color.black;

	private GameplayController gameplayController;

	private TrailRenderer trailRenderer;

	private float delay = .1f;

	public Vector3 thrustDirection;

	public Transform hit_base;

	public Transform skin;

	private bool isInit;


	public bool isHolding;

	public bool holdStatus;



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

		targetLocation = transform.position;

		targetLocalPosition = transform.localPosition;
	}

	void OnEnable()
	{

		EventManager.OnBoostStart += OnBoostStart;

		EventManager.OnHoldStatus += OnHoldStatus;
	}

	void OnDisable()
	{


		EventManager.OnBoostStart -= OnBoostStart;


		EventManager.OnHoldStatus -= OnHoldStatus;

	}


	public void Init()
	{

		line = transform.GetChild(0).GetComponent<LineController>();

		objectSpawner = ObjectSpawner.Instance;

		if (currentBase != null)
		{
			currentBase.player = this;
		}


		defaultScale = transform.localScale.x;

		targetLocation = transform.localPosition;

		cameraController = Camera.main.GetComponent<CameraController>();

		renderer = transform.GetChild(2).GetComponent<SpriteRenderer>();

		gameplayController = GameplayController.Instance;

		trailRenderer = GetComponent<TrailRenderer>();

		SetCurrentBase();

		isInit = true;

	}


	void Update ()
	{
		if (GameplayController.GAME_STATE != State.GAME)
		{
			return;
		}

		if (currentBase == null) { return; }


		transform.right = transform.position - currentBase.transform.position;

		scale = (defaultScale / currentBase.transform.localScale.x);

		transform.localScale = new Vector3(scale, scale, scale);

		skin.transform.position = line.line.GetPosition(1) + (line.transform.right * .2f);

		// if (Input.GetKeyDown(KeyCode.L))
		// {
		// 	objectSpawner.SpawnParticle(ParticleType.SMOKE, currentBase.transform.position);

		// 	fan.transform.position = currentBase.transform.position;

		// 	fan.Play();
		// }

		if (Input.GetKeyDown(KeyCode.J))
		{

			if (EventManager.OnBoostStart != null)
			{
				EventManager.OnBoostStart();
			}
		}


	}

	void OnBoostStart()
	{

		StopCoroutine("IStartBoost");

		StartCoroutine("IStartBoost");

	}

	private IEnumerator IStartBoost()
	{

		idealScore = ((int)(GameplayController.SCORE) + boostScoreAddition) - 1;

		trailRenderer.enabled = true;

		gameplayController.hasUsedBoost = true;

		gameplayController.boostActive = activeBoost = true;

		bsm.Activate();

		float currentScore = GameplayController.SCORE;

		BaseController.VELOCITY_SCALE = 0f;

		spikes.gameObject.SetActive(true);

		spikes.position = Camera.main.ViewportToWorldPoint(new Vector2(.5f, 0));

		spikes.GetComponent<Particle>().Play();

		boostPrism.gameObject.SetActive(true);

		boostPrism.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(.5f, 0));

		boostPrism.Play();

		transform.position = objectSpawner.nextBase.transform.position;

		Time.timeScale = 1f;

		Time.fixedDeltaTime = Time.timeScale * .02f;

		float dividor = boostScoreAddition / 4f;

		float time = 0;

		while (bsm.isPlaying())
		{
			GameplayController.SCORE +=  Time.deltaTime * dividor;

			transform.position = objectSpawner.nextBase.transform.position;

			time += Time.unscaledDeltaTime;

			time = Mathf.Clamp(time, .5f, 1.34f);

			Time.timeScale = time;

			Time.fixedDeltaTime = Time.timeScale * .02f;


			line.Shoot();

			yield return null;
		}

		gameplayController.boostActive = activeBoost = false;

		currentBase.shouldRotate = true;

		Round();

		BaseController.VELOCITY_SCALE = 1f;


		spikes.GetComponent<Particle>().Stop();

		spikes.gameObject.SetActive(false);

		boostPrism.gameObject.SetActive(false);

		boostPrism.Stop();

		Time.timeScale = 1f;

		Time.fixedDeltaTime = Time.timeScale * .02f;

		FindObjectOfType<CameraController>().Twirl();

		GameplayController.SCORE = idealScore;

		trailRenderer.enabled = false;

		if (EventManager.OnBoostEnd != null)
		{

			EventManager.OnBoostEnd();

		}

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

		float radius = .5f;

		float angle = (int)CalculateAngle(transform.position, currentBase.transform.position);

		float y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

		float xx = radius * Mathf.Cos(angle * Mathf.Deg2Rad);

		Vector3 pos = new Vector3(xx, y, 0);

		transform.localPosition  = pos;
	}

	public  float CalculateAngle(Vector3 from, Vector3 to)
	{
		return Quaternion.FromToRotation(-currentBase.transform.right, to - from).eulerAngles.z;
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

		currentBase.visited = true;

		currentBase.transform.gameObject.layer = 9;
	}


	void OnCollisionEnter2D(Collision2D col)
	{
		if (GameplayController.GAME_STATE != State.GAME || hit_base == null) { return; }

		if (col.gameObject == currentBase.transform.gameObject)
		{

			float shake = Random.Range(3.5f, 3.8f);

			float shrinkFactor = Random.Range(9.0f, 10.0f);

			cameraController.Jitter(shake, shrinkFactor);

			cameraController.SetYOffset(thrustDirection);


			SpawnEffect();

		}
	}




	public void SetColor(Color c)
	{
		if (!isInit)
		{
			Init();
		}
		for (int i = 0; i < particleSystems.Length; i++)
		{
			particleSystems[i].startColor = c;
		}
		line.line.SetColors(c, c);

		renderer.color = c;

		trailRenderer.startColor = trailRenderer.endColor = c;
	}


	void SpawnEffect()
	{

		if (GameplayController.GAME_STATE != State.GAME) { return; }


		if (renderer.color.r == 0)
		{
			blackBase.transform.position = currentBase.transform.position;

			blackBase.Play();

		}
		else
		{
			if (activeBoost) { return; }

			fan.startColor = renderer.color;

			fan.transform.position = currentBase.transform.position;

			fan.Play();

		}
	}

	public void SetTargetLocalPosition(Vector3 position)
	{

		this.targetLocalPosition = position;

	}

	void OnHoldStatus(int i)
	{
		if (i == 1)
		{
			slowmotionParticle.gameObject.SetActive(true);

			slowmotionParticle.Play();

			slowmotionParticle.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(.5f, 0f)) + new Vector3(0, 0, 10);
		}
		else
		{
			slowmotionParticle.Stop();

			slowmotionParticle.gameObject.SetActive(false);

			if (!activeBoost)
			{

				Time.timeScale = 1f;

				Time.fixedDeltaTime = Time.timeScale * .02f;

			}
		}
	}

	public int IdealScore
	{
		get {
			return idealScore;
		}
	}
}



