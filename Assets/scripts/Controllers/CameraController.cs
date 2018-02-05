using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MK.Glow;
using  UnityStandardAssets.ImageEffects;
public class CameraController : MonoBehaviour {

	
	public bool freezeCamera;
	
	public Color defaultBackgroundColor;

	public Transform backgroundSmoke; 

	
	private float jitterAmount;
	
	private float shakeAmount;
	
	private float decreaseFactor;
	
	private float vel;
	
	private float glowVel;
	
	private float def_glowintensity;
	
	private float vortexCenterPosition = 1f;
	
	private float vortexVel;
	
	private float vortexAngle = 1F;
	
	private float vortexAngleVel;
	
	private Vortex vortex;
	
	private MKGlowFree mkglow;
	
	private PlayerController player;
	
	private ObjectSpawner spawner;
	
	private List<Vector3> positions = new List<Vector3>();
	
	private Camera camera;

	public Transform ps; 

	private float yOffset; 

	private float yOffsetVel; 

	private Vector3 hitOffset; 

	private Twirl[] twirl; 


	void OnEnable()
	{
		
		EventManager.OnBaseHit += OnBaseHit;


		EventManager.OnGameStart+=OnGameStart; 

		EventManager.OnGameOver+=OnGameOver; 
	}

	void OnDisable()
	{
		EventManager.OnBaseHit -= OnBaseHit;

		
		EventManager.OnGameStart-=OnGameStart; 

		EventManager.OnGameOver-=OnGameOver; 		
	}

	void Start ()
	{
		
		camera = Camera.main;
		
		player = PlayerController.Instance;
		
		Camera.main.orthographicSize = 4f * Screen.height / Screen.width * 0.5f;
		
		spawner = ObjectSpawner.Instance;
		
		positions.Add(player.transform.position);
		
		positions.Add(spawner.transform.position);
		
		mkglow = transform.GetComponent<MKGlowFree>();
		
		def_glowintensity = mkglow.GlowIntensityInner;
		
		defaultBackgroundColor = camera.backgroundColor;
		
		vortex = GetComponent<Vortex>();

		twirl = GetComponents<Twirl>(); 
		
		SetGlow(.118f); 

	}

	void FixedUpdate ()
	{
		
		mkglow.GlowIntensityInner = Mathf.SmoothDamp(mkglow.GlowIntensityInner, def_glowintensity, ref glowVel, Time.deltaTime * 25f);
		
		if(Input.GetKeyDown(KeyCode.R))
		{
			Twirl(); 
		}

		if(GameplayController.GAME_STATE != State.GAME) return; 
		
		if (freezeCamera) { return; }
		
		if (spawner.nextBase == null) { return; }
		
		positions[0] = player.currentBase.transform.position;
		
		positions[1] = spawner.nextBase.transform.position;
		
		Vector2 jitter = JitterCamera();

		hitOffset = Vector3.Lerp(hitOffset, Vector2.zero, Time.deltaTime * 10f); 

		yOffset = Mathf.SmoothDamp(yOffset, 0, ref yOffsetVel, Time.deltaTime * 5f); 
		
		Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, GetDistance(), ref vel, Time.deltaTime * 15f);
		
		transform.position = Vector3.Lerp(transform.position, GetAveragePosition() +  new Vector3(0, 0, -10f) + new Vector3(jitter.x, 0, -10f) + hitOffset, Time.deltaTime * 4.0f);

		backgroundSmoke.position = transform.position + new Vector3(0, 0, 10); 

		//ps.position = Camera.main.ViewportToWorldPoint(new Vector2(.5f, 0)); 

	}

	Vector3 GetAveragePosition()
	{
		
		float xSum = 0;
		
		float ySum = 0;
		
		Vector3 average = Vector3.zero;
		
		for (int i = 0 ; i < positions.Count; i++)
		{
			xSum += positions[i].x;
			
			ySum += positions[i].y;
		}

		
		xSum /= positions.Count;
		
		ySum /= positions.Count;

		average = new Vector3(xSum, ySum, -10);
		
		return average;
	}

	void OnGameStart()
	{
		SetGlow(.418f); 
	}

	void OnGameOver()
	{

	}

	void OnBaseHit()
	{
		SetGlow(.118f); 
		Vibration.Vibrate(GameplayController.VIBRATION_DURATION + 1);			
		

	}


	float GetDistance()
	{
		
		float averageSize = (player.currentBase.size + spawner.nextBase.size) / 10f;
		
		return 	Vector2.Distance(player.currentBase.transform.position, positions[1]) + averageSize;
	}

	Vector2 JitterCamera()
	{
		
		Vector2 offset = Random.insideUnitCircle * jitterAmount;
		
		jitterAmount -= Time.deltaTime * decreaseFactor;
		
		jitterAmount = Mathf.Clamp(jitterAmount, 0, jitterAmount);
		
		return offset;
	}

	Vector3 BoostShake()
	{
		float min = 0; 

		min = Mathf.Clamp(min, .01f, min);
		return new Vector3(Mathf.PingPong(Time.time * 80f, 4f) - 2f, 0, 0); 
	}


	public void Twirl()
	{
		ToggleTwirl(true); 
		StopCoroutine("ITwirl"); 
		StartCoroutine("ITwirl"); 
	}

	private IEnumerator ITwirl()
	{

		float va = .25f; 
		float velocity = 1.9f; 
		float limit = .75f; 
		float aspect = (float)Screen.height / (float)(Screen.width); 
		CameraLerpColor(); 
		while(va < 1.0f)
		{
			va+=velocity * Time.unscaledDeltaTime; 
			velocity-=Time.unscaledDeltaTime * 3f; 
			velocity = Mathf.Clamp(velocity, .4f, velocity); 

			va = Mathf.Clamp(va, 0f, 1f);
			twirl[0].radius = new Vector2(va * aspect, va) * limit; 
			twirl[0].angle = (va * 360f);
			twirl[0].angle = Mathf.Clamp(twirl[0].angle, 0f, 360f); 

			twirl[1].radius = new Vector2(va * aspect, va) * limit; 
			twirl[1].angle =  360 -(va * 360f);
			twirl[1].angle = Mathf.Clamp(twirl[1].angle, 0f, 360f); 

			yield return null; 
		}

		ToggleTwirl(false); 
	}

	public void CameraLerpColor()
	{
		StopCoroutine("ICameraLerpColor"); 

		StartCoroutine("ICameraLerpColor"); 
	}


	IEnumerator ICameraLerpColor()
	{
		camera.backgroundColor = Color.white; 

		while(camera.backgroundColor != defaultBackgroundColor)
		{
			camera.backgroundColor = Color.Lerp(camera.backgroundColor, defaultBackgroundColor, Time.unscaledDeltaTime * 2f); 
			yield return null; 
		}
	}


	public void SetYOffset(Vector3 forward)
	{
		hitOffset = forward; 
	}

	public void Jitter(float amount, float decreaseFactor)
	{
		
		jitterAmount = amount;
		
		this.decreaseFactor = decreaseFactor;
	}

	public void SetGlow(float g)
	{
		mkglow.GlowIntensityInner = g;
	}


	private void ToggleTwirl(bool b)
	{
		twirl[0].enabled = twirl[1].enabled = b; 
	}


}
