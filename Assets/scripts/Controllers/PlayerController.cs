using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{


	public static PlayerController Instance;

	public bool activeBoost;


	public BoostSpriteMask bsm;

	public BaseController currentBase;

	public ParticleSystem[] particleSystems;

	public Particle slowmotionParticle; 

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

	private int boostScoreAddition = 20;

	private Color scheme = Color.black;

	private GameplayController gameplayController;

	private TrailRenderer trialRenderer;

	private float delay = .1f;

	public Vector3 thrustDirection;

	public Transform hit_base;

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

		EventManager.OnGameStart += OnGameStart;

		EventManager.OnHoldStatus+=OnHoldStatus; 
	}

	void OnDisable()
	{


		EventManager.OnBoostStart -= OnBoostStart;

		EventManager.OnGameStart -= OnGameStart;

		EventManager.OnHoldStatus-=OnHoldStatus; 

	}

	bool gameStart;
	void OnGameStart()
	{
		gameStart = true;
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

		trialRenderer = GetComponent<TrailRenderer>();


		//trialRenderer.SetColor(Color.white, Color.white); 

		SetCurrentBase();

		isInit = true; 

	}


	void Update ()
	{
		if (currentBase == null) { return; }

		transform.right = transform.position - currentBase.transform.position;

		scale = (defaultScale / currentBase.transform.localScale.x);

		transform.localScale = new Vector3(scale, scale, scale);


		if (Input.GetKeyDown(KeyCode.L))
		{
			objectSpawner.SpawnParticle(ParticleType.SMOKE, currentBase.transform.position);

			fan.transform.position = currentBase.transform.position;

			fan.Play();
		}

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

		trialRenderer.enabled = true;


		activeBoost = true;

		bsm.Activate();

		int currentScore = GameplayController.SCORE;

		BaseController.VELOCITY_SCALE = 0f;

		spikes.gameObject.SetActive(true);

		spikes.position = Camera.main.ViewportToWorldPoint(new Vector2(.5f, 0));

		spikes.GetComponent<Particle>().Play();


		boostPrism.gameObject.SetActive(true);

		boostPrism.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(.5f, 0));

		boostPrism.Play();




		transform.position = objectSpawner.nextBase.transform.position;

		Time.timeScale = 2f;

		Time.fixedDeltaTime = Time.timeScale * .02f;


		while (GameplayController.SCORE != currentScore + boostScoreAddition)
		{

			transform.position = objectSpawner.nextBase.transform.position;

			line.Shoot();


			//Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 2f, ref timeVel, Time.unscaledDeltaTime * 75f);

			//Time.fixedDeltaTime = Time.timeScale * .02f;

			spikes.position = Camera.main.ViewportToWorldPoint(new Vector2(.5f, 0));

			boostPrism.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(.5f, .1f)) + new Vector3(0,0,1f);


			float shake = Random.Range(3.5f, 4.0f);

			float shrinkFactor = Random.Range(6.0f, 7.5f);

			cameraController.Jitter(shake, shrinkFactor);

			yield return null;

		}

		currentBase.shouldRotate = true;

		Round();

		BaseController.VELOCITY_SCALE = 1f;

		activeBoost = false;

		spikes.GetComponent<Particle>().Stop();

		spikes.gameObject.SetActive(false);

		boostPrism.gameObject.SetActive(false);

		boostPrism.Stop();

		//cameraController.SetGlow(.318f);

		Time.timeScale = 1f;

		Time.fixedDeltaTime = Time.timeScale * .02f;

		FindObjectOfType<CameraController>().Twirl();


		//objectSpawner.nextBase.SetColor(Color.black);

		//SetColor(Color.black);

		trialRenderer.enabled = false;

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

			if (!activeBoost)
			{

				float shake = Random.Range(2.1f, 2.8f);

				float shrinkFactor = Random.Range(6.0f, 6.5f);

				cameraController.Jitter(shake, shrinkFactor);

				cameraController.SetYOffset(thrustDirection);

				//Vibration.Vibrate(GameplayController.VIBRATION_DURATION);


			//s	InvertColors();
			}

			SpawnEffect(); 

//			cameraController.SetGlow(base_hit_glow);
		}
	}


	// void InvertColors()
	// {


	// 	Color current = hit_base.GetComponent<SpriteRenderer>().color;

	// 	Color invert = current.r == 0 ? Color.white : Color.black;

	// 	SetColor(invert);

	// 	SpawnEffect();
	// }

	public void SetColor(Color c)
	{
		if(!isInit)
		{
			Init(); 
		}
		for (int i = 0; i < particleSystems.Length; i++)
		{
			particleSystems[i].startColor = c;
		}

		//currentBase.SetColor(c);

		line.line.SetColors(c, c);

		renderer.color = c;
	}


	void SpawnEffect()
	{

		if(GameplayController.GAME_STATE != State.GAME) return; 


		if (renderer.color.r == 0)
		{
			objectSpawner.SpawnParticle(ParticleType.SMOKE, currentBase.transform.position);

			blackBase.transform.position = currentBase.transform.position;

			blackBase.Play();

		}
		else
		{
			if (!activeBoost)
			{
				currentBase.SpawnRing(renderer.color);

				fan.startColor = renderer.color;

				fan.transform.position = currentBase.transform.position;

				fan.Play();
			}
		}
	}

	public void SetTargetLocalPosition(Vector3 position)
	{

		this.targetLocalPosition = position;

	}

	void OnHoldStatus(int i)
	{
		if(i == 1)
		{
			slowmotionParticle.gameObject.SetActive(true); 

			slowmotionParticle.Play(); 

			slowmotionParticle.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(.5f, 0f)) + new Vector3(0, 0, 10); 
		}
		else
		{
			slowmotionParticle.Stop(); 

			slowmotionParticle.gameObject.SetActive(false); 

			if(!activeBoost)
			{
				
				Time.timeScale = 1f;

				Time.fixedDeltaTime = Time.timeScale * .02f;

			}
		}
	}
}



