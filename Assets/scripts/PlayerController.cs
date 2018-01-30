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
	
	public Transform spikes;

	public ParticleSystem fan; 
	
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
	
	private Color scheme = Color.black;

	private GameplayController gameplayController;


	void Awake()
	{
		if(Instance == null)
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
		
		EventManager.OnBaseHit += OnBaseHit;
		
		EventManager.OnBoostEnd += OnBoostEnd;
		
		EventManager.OnBoostStart += OnBoostStart;

		EventManager.OnGameStart+=OnGameStart; 
	}

	void OnDisable() 
	{
		
		EventManager.OnBaseHit -= OnBaseHit;
		
		EventManager.OnBoostEnd -= OnBoostEnd;
		
		EventManager.OnBoostStart -= OnBoostStart;

		EventManager.OnGameStart-=OnGameStart; 

	}

	void Start ()
	{
		Init(); 
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
		
		SetCurrentBase();


	}


	void Update ()
	{
		if(currentBase == null) return; 
		
		transform.right = transform.position - currentBase.transform.position;
		
		scale = (defaultScale / currentBase.transform.localScale.x);
		
		transform.localScale = new Vector3(scale, scale, scale);


		if (Input.GetKeyDown(KeyCode.L))
		{
			objectSpawner.SpawnParticle(ParticleType.SMOKE, currentBase.transform.position); 

			fan.transform.position = currentBase.transform.position; 

			fan.Play(); 

			// if(EventManager.OnBoostStart != null)
			// {
			// 	EventManager.OnBoostStart(); 
			// }
		}

		if (Input.GetKeyDown(KeyCode.J))
		{

			if(EventManager.OnBoostStart != null)
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

	void OnBoostEnd()
	{

	}

	private IEnumerator IStartBoost()
	{
		
		
		activeBoost = true; 
		
		bsm.Activate();

		int currentScore = GameplayController.SCORE;
		
		BaseController.VELOCITY_SCALE = 0f; 
		
		spikes.gameObject.SetActive(true); 

		spikes.position = Camera.main.ViewportToWorldPoint(new Vector2(.5f, 0)); 

		
		spikes.GetComponent<Particle>().Play(); 

		transform.position = objectSpawner.nextBase.transform.position;
		
		// Time.timeScale = .5f;

		// Time.fixedDeltaTime = Time.timeScale * .02f;

		
		while(GameplayController.SCORE != currentScore + boostScoreAddition)
		{

			transform.position = objectSpawner.nextBase.transform.position;

			line.Shoot(); 

			Round(); 

			// SetTargetBase(objectSpawner.nextBase); 

			// gameplayController.IncrementScore();
			// if (EventManager.OnBaseHit != null)
			// {
			// 	EventManager.OnBaseHit();
			// }

			spikes.position = Camera.main.ViewportToWorldPoint(new Vector2(.5f, 0)); 

			float shake = Random.Range(3.5f, 4.0f);

			float shrinkFactor = Random.Range(6.0f, 7.5f);

			cameraController.Jitter(shake, shrinkFactor);

			Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 1.5f, ref timeVel, Time.deltaTime * 75f);

			Time.fixedDeltaTime = Time.timeScale * .02f;

			yield return null; 
		}
		
		BaseController.VELOCITY_SCALE = 1f;
		
		activeBoost = false;
		
		spikes.GetComponent<Particle>().Stop();
		
		spikes.gameObject.SetActive(false);
		
		cameraController.SetGlow(.118f);
		
		Time.timeScale = 1f; 
		
		Time.fixedDeltaTime = Time.timeScale * .02f;
		
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

	void OnBaseHit()
	{
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
		if(GameplayController.GAME_STATE != State.GAME) return;

		if (col.gameObject == currentBase.transform.gameObject)
		{
			if (!activeBoost)
			{
				
				float shake = Random.Range(2.0f, 2.3f);
				
				float shrinkFactor = Random.Range(6.0f, 6.5f);
				
				Camera.main.transform.GetComponent<CameraController>().Jitter(shake, shrinkFactor);
			}


			scheme = scheme.r == 0f ? Color.white : Color.black; 
			
			//objectSpawner.SpawnParticle(ParticleType.BOOM, currentBase.transform.position, scheme);

			InvertColors();		
		}
	}


	void InvertColors()
	{
		
		Color current = renderer.color;
		
		Color invert = scheme;

		for(int i = 0;i < particleSystems.Length; i++)
		{
			particleSystems[i].startColor = invert; 
		}

		currentBase.SetColor(invert);
		
		line.line.SetColors(scheme, scheme);
		
		cameraController.SetGlow(.118f);

		renderer.color = invert;

		if(renderer.color.r == 0)
		{
			objectSpawner.SpawnParticle(ParticleType.SMOKE, currentBase.transform.position); 

			fan.transform.position = currentBase.transform.position; 

			fan.Play(); 
		}
		else
		{
			currentBase.SpawnRing(scheme);
		}
	}

	public void SetTargetLocalPosition(Vector3 position)
	{
		
		this.targetLocalPosition = position;

	}
}



