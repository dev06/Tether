using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour {

	public static LineController Instance;

	
	private Vector3 endLinePosition;
	
	private Vector3 startLinePosition;


	
	private float defaultLineWidth = .08f;
	
	private float lineWidthVel;
	
	private float retractTimer = 0;
	
	private ObjectSpawner objectSpawner;
	
	private GameplayController gameplayController;

	
	private bool attached = false;
	
	private bool endLineToPlayer = true;
	
	private bool hitSomething = false;


	public float speed = 3.0f;

	public PlayerController player;

	public LineCollider collider;

	public GameObject currentBase;

	public Vector3 hitPoint;

	public LineRenderer line;

	private float length = 2.0f;

	private float time = 1f;

	private float holdStartDelayTimer; 

	private float holdTimer; 

	private bool pressed; 

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
	void Start ()
	{
		line = GetComponent<LineRenderer>();

		player = PlayerController.Instance;
		
		endLinePosition = player.transform.position;
		
		startLinePosition = player.transform.position;
		
		hitPoint = player.transform.position;
		
		line.startWidth = line.endWidth = defaultLineWidth;
		
		objectSpawner = ObjectSpawner.Instance;
		
		gameplayController = GameplayController.Instance;
		
		line.startWidth = line.endWidth = 0; 
	}

	float timeVel;

	void Update ()
	{
		if(GameplayController.GAME_STATE != State.GAME) return; 

		transform.right = player.transform.right;

		if (!attached)
		{
			startLinePosition = player.transform.position;
		}

		if (hitSomething)
		{
			endLinePosition = hitPoint;
		}

		if (endLineToPlayer)
		{
			endLinePosition = player.transform.position;
		}

		
		line.SetPosition(0, startLinePosition);
		
		line.SetPosition(1, endLinePosition);
		
		line.endWidth = Mathf.SmoothDamp(line.endWidth, defaultLineWidth, ref lineWidthVel, Time.deltaTime * 20f);

		if(Input.GetMouseButton(0) && !player.activeBoost)
		{
			holdStartDelayTimer+=Time.unscaledDeltaTime; 

			if(holdStartDelayTimer > .6f)
			{
				if(!inSlowmo)
				{
					if(EventManager.OnHoldStatus != null)
					{
						EventManager.OnHoldStatus(1); 
					}
				}
				inSlowmo = true;

				player.isHolding = true; 				
				Time.timeScale = .3f;
				Time.fixedDeltaTime = Time.timeScale * .02f; 
			}

		}

		if (Input.GetMouseButtonUp(0))
		{
			
			player.isHolding = false ;
			holdStartDelayTimer = 0; 
			Time.timeScale = 1f; 
			Time.fixedDeltaTime = Time.timeScale * .02f; 
			Shoot();
			if(inSlowmo)
			{
				if(EventManager.OnHoldStatus != null)
				{
					EventManager.OnHoldStatus(0); 
				}
			}
			inSlowmo = false; 
		}
	}

	bool inSlowmo; 
	public  float CalculateAngle(Vector3 from, Vector3 to)
	{
		return Quaternion.FromToRotation(Vector3.right, from - to).eulerAngles.z;
	}

	public LayerMask layer, allHit;

	private IEnumerator IStartPowerUp()
	{

		yield return null; 

		player.activeBoost = true;

		player.bsm.Activate();

		Time.timeScale = .7f;

		Time.fixedDeltaTime = Time.timeScale * .02f;

		if (EventManager.OnBoostStart != null)
		{
			EventManager.OnBoostStart();
		}

	}

	

	public void Shoot()
	{
		if (attached) { return; }

		float tether_legth = 15f;

		RaycastHit2D hit = Physics2D.Raycast(transform.position,  transform.right, tether_legth, layer);

		if(!player.activeBoost)
		{
			RaycastHit2D all = Physics2D.Raycast(transform.position,  transform.right, tether_legth, allHit);

			if (all.collider != null)
			{
				if (all.transform.gameObject.tag == "Objects/powerup" && !player.activeBoost)
				{

					all.transform.gameObject.SetActive(false);

					StopCoroutine("IStartPowerUp"); 

					StartCoroutine("IStartPowerUp");

				}
			}
		}

		player.currentBase.shouldRotate = false; 	



		startLinePosition = player.transform.position;



		endLineToPlayer = false;

		hitSomething = (hit.collider != null);

		line.startWidth = line.endWidth = defaultLineWidth;

		if (hit.collider != null)
		{

			float a = CalculateAngle(hit.point, hit.transform.position);

			BaseController.DIRECTION = (a > 0 && a < 270) ? -1 : 1;

			hitPoint = hit.point;

			endLinePosition = hit.point;

			line.endWidth = .25f;

			player.hit_base = hit.transform; 
			//Camera.main.GetComponent<CameraController>().SetYOffset(transform.right * 20f); 

			player.thrustDirection = transform.right * Random.Range(5f, 11f); 

			Camera.main.GetComponent<CameraController>().SetYOffset(-player.thrustDirection * .5f); 




			object[] objs = new object[2] {hit.transform.GetComponent<BaseController>(), hit.point};

			StopCoroutine("Retract");

			StopCoroutine("Attach");

			StartCoroutine("Attach", objs);

		}
		else
		{
			endLinePosition = transform.position + (transform.right * tether_legth);

			line.SetPosition(1, endLinePosition);
		}


		if (!hitSomething)
		{
			Time.timeScale = 1f; 
			Time.fixedDeltaTime = Time.timeScale * .02f; 

			StopCoroutine("Retract");

			StartCoroutine("Retract");
		}
	}

	IEnumerator Attach(object[] objs)
	{

		BaseController hitBase = (BaseController)objs[0];

		Vector2 hitPosition = (Vector2)objs[1];

		attached = true;

		bool gameOver = false; 

		if (hitBase != null)
		{

			player.SetTargetBase(hitBase);

			gameplayController.IncrementScore();

			hitBase.SetFreeze(true);



			if (EventManager.OnBaseHit != null)
			{
				EventManager.OnBaseHit();
			}

		} 
		else
		{
			if (!player.activeBoost)
			{

				Camera.main.GetComponent<CameraController>().freezeCamera = true;

				Time.timeScale = .1f;

				Time.fixedDeltaTime = Time.timeScale * .02f; 


				StopCoroutine("LerpTimeToNormal"); 

				StartCoroutine("LerpTimeToNormal"); 

				gameOver = true; 
			}
		}


		player.currentBase.shouldRotate = false;


		endLineToPlayer = false;

		yield return new WaitForSeconds(.15f);

		hitSomething = false;

		while (Mathf.Abs(Vector2.Distance(hitPosition, startLinePosition)) > .01f)
		{

			startLinePosition = Vector3.Lerp(startLinePosition, hitPosition, Time.deltaTime * 20f);

			player.SetLocation(startLinePosition);

			yield return null;
		}


		attached = false;

		player.currentBase.shouldRotate = true;

		endLineToPlayer = true;

		player.currentBase.transform.gameObject.layer = 9;

		player.Round();

		if(gameOver)
		{
			if (EventManager.OnGameOver != null)
			{
				EventManager.OnGameOver();
			}
		}
	}

	IEnumerator Retract()
	{


		while (Mathf.Abs(Vector2.Distance(endLinePosition, player.transform.position)) > .07f)
		{

			endLinePosition = Vector3.Lerp(endLinePosition, player.transform.position, Time.deltaTime * 10f);

			yield return null;
		}

		BaseController.VELOCITY_SCALE = 1F; 


		player.currentBase.shouldRotate = true; 

		endLineToPlayer = true;
	}


	IEnumerator LerpTimeToNormal()
	{
		float timeScaleVel = 0; 

		while(Time.timeScale < 1)
		{
			Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 1f, ref timeScaleVel, Time.unscaledDeltaTime * 6f); 

			Time.fixedDeltaTime = Time.timeScale * .02f; 

			yield return null; 
		}


	}

}
