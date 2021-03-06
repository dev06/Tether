﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour {

	public static LineController Instance;


	private Vector3 endLinePosition;

	private Vector3 startLinePosition;

	private bool isInit;

	private float defaultLineWidth = .13f;

	private float lineWidthVel;

	private float retractTimer = 0;

	private ObjectSpawner objectSpawner;

	private GameplayController gameplayController;


	private bool attached = false;

	private bool endLineToPlayer = true;

	private bool hitSomething = false;

	private bool shotTether = false;

	private bool inSlowmo;


	private Vector2 hitPosition;

	public float speed = 3.0f;

	public PlayerController player;

	public LineCollider collider;

	public GameObject currentBase;

	public Vector3 hitPoint;

	public LineRenderer line;

	private float length = 2.0f;

	private float controlTimer = 0; // controls when game controls are activated upon game start

	private float holdStartDelayTimer;

	private float holdTimer;

	private bool pressed;

	public int baseHitCounter;


	private LockTaskPanel locktaskpanel;

	public LayerMask layer, allHit;

	void OnEnable()
	{
		EventManager.OnUnpause += OnUnpause;
	}

	void OnDisable()
	{
		EventManager.OnUnpause -= OnUnpause;
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
		line = GetComponent<LineRenderer>();

		line.enabled = false;

		player = PlayerController.Instance;

		endLinePosition = player.transform.position;

		startLinePosition = player.transform.position;

		hitPoint = player.transform.position;

		line.startWidth = line.endWidth = defaultLineWidth;

		objectSpawner = ObjectSpawner.Instance;

		gameplayController = GameplayController.Instance;

		line.startWidth = line.endWidth = 0;

		locktaskpanel = FindObjectOfType<LockTaskPanel>();


		isInit = true;
	}

	void LateUpdate()
	{

	}

	void Update ()
	{
		if (GameplayController.GAME_STATE == State.PAUSE) { return; }

		if (GameplayController.GAME_STATE != State.GAME) { return; }


		if (controlTimer < 1)
		{
			controlTimer += Time.unscaledDeltaTime;
		}

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

		if (Input.GetMouseButton(0) && !player.activeBoost)
		{
			holdStartDelayTimer += Time.unscaledDeltaTime;

			if (holdStartDelayTimer > .6f)
			{
				if (!inSlowmo)
				{
					if (EventManager.OnHoldStatus != null)
					{
						EventManager.OnHoldStatus(1);
					}
				}
				inSlowmo = true;


				player.isHolding = true;

				Time.timeScale = .2f;

				Time.fixedDeltaTime = Time.timeScale * .02f;
			}

		}

		if (Input.GetMouseButtonUp(0) && controlTimer > .2f)
		{

			player.isHolding = false ;

			holdStartDelayTimer = 0;

			Shoot();

			if (inSlowmo)
			{
				if (EventManager.OnHoldStatus != null)
				{
					EventManager.OnHoldStatus(0);
				}
			}

			inSlowmo = false;
		}
	}


	public  float CalculateAngle(Vector3 from, Vector3 to)
	{
		return Quaternion.FromToRotation(Vector3.right, from - to).eulerAngles.z;
	}

	bool running;
	public void Shoot()
	{
		if (attached) { return; }

		line.enabled = true;

		float tether_legth = 100f;

		Vector3 start = player.transform.position;
		Vector3 dir = player.transform.right;
		RaycastHit2D hit = Physics2D.Raycast(start, dir, tether_legth, layer);

		hitSomething = (hit.collider != null);

		if (!player.activeBoost && hitSomething)
		{

			if (hit.transform.gameObject.tag == "Objects/powerup")
			{

				if (EventManager.OnBoostStart != null)
				{
					EventManager.OnBoostStart();
				}
			}

		}

		player.currentBase.shouldRotate = false;

		startLinePosition = player.transform.position;

		endLineToPlayer = false;

		line.startWidth = line.endWidth = defaultLineWidth;

		if (hitSomething)
		{

			if (EventManager.OnBaseHitPerfection != null)
			{
				float theta = Vector3.Angle(player.transform.position - hit.transform.position, -player.transform.right);
				EventManager.OnBaseHitPerfection(theta);
			}
			float a = CalculateAngle(hit.point, hit.transform.position);

			BaseController.DIRECTION = (a > 0 && a < 270) ? -1 : 1;

			hitPoint = hit.point;

			endLinePosition = hit.point;

			line.endWidth = .5f;

			player.hit_base = hit.transform;

			player.thrustDirection = transform.right * (inSlowmo ? 18f : 6f);

			object[] objs = new object[3] {hit.transform.GetComponent<BaseController>(), hit.point, hit.transform.gameObject};

			// StopCoroutine("Retract");

			// StopCoroutine("Attach");

			if (!running)
			{

				StartCoroutine("Attach", objs);
			}

			Color color = GameplayController.LevelIndex == 0 ? Color.white : Color.black;

			objectSpawner.SpawnParticle(ParticleType.ONLINEHITBASE, hit.point, color);

			if (hit.transform.GetComponent<BaseController>() != null)
			{
				baseHitCounter++;

				if (baseHitCounter >= LockTaskValue.Task2Value)
				{
					if (locktaskpanel != null)
					{

						locktaskpanel.InvokeLockTask(LockTaskID.ID_2);
					}
				}
			}

		}
		else
		{
			endLinePosition = transform.position + (transform.right * tether_legth);

			line.SetPosition(1, endLinePosition);

			baseHitCounter = 0;

			StopCoroutine("Retract");

			StartCoroutine("Retract");

		}

	}

	IEnumerator Attach(object[] objs)
	{
		running = true;

		BaseController hitBase = (BaseController)objs[0];

		Vector2 hitPosition = (Vector2)objs[1];

		GameObject g = (GameObject)objs[2];

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
			if (g.tag == "Objects/border")
			{
				if (!player.activeBoost)
				{

					Camera.main.GetComponent<CameraController>().freezeCamera = true;

					Time.timeScale = .2f;

					Time.fixedDeltaTime = Time.timeScale * .02f;

					gameOver = true;

					// StopCoroutine("LerpTimeToNormal");

					// StartCoroutine("LerpTimeToNormal");
				}
			}

		}


		player.currentBase.shouldRotate = false;


		endLineToPlayer = false;

		yield return new WaitForSeconds(.1f);

		hitSomething = false;

		while (Mathf.Abs(Vector2.Distance(hitPosition, startLinePosition)) > .02f)
		{

			startLinePosition = Vector3.Lerp(startLinePosition, hitPosition, Time.unscaledDeltaTime * 25f);

			player.SetLocation(startLinePosition);

			yield return null;
		}

		attached = false;


		player.currentBase.shouldRotate = true;

		endLineToPlayer = true;

		player.currentBase.transform.gameObject.layer = 9;

		player.Round();

		line.enabled = false;

		running = false;

		if (gameOver)
		{
			if (EventManager.OnGameOver != null)
			{
				EventManager.OnGameOver();
			}
		}
	}

	public void SetEndLinePosition(Vector3 position)
	{
		endLinePosition = position;

		line.SetPosition(1, position);

		endLineToPlayer = false;
	}


	IEnumerator Retract()
	{

		float difference = Mathf.Abs(Vector2.Distance(endLinePosition, player.transform.position));

		while (difference > .07f)
		{

			endLinePosition = Vector3.Lerp(endLinePosition, player.transform.position, Time.deltaTime * 15f);

			difference = Mathf.Abs((endLinePosition - player.transform.position).magnitude);

			yield return null;
		}

		BaseController.VELOCITY_SCALE = 1F;

		player.currentBase.shouldRotate = true;

		endLineToPlayer = true;

		line.enabled = false;
	}


	IEnumerator LerpTimeToNormal()
	{
		float timeScaleVel = 0;

		while (Time.timeScale < 1)
		{
			Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 1f, ref timeScaleVel, Time.unscaledDeltaTime * 15f);

			Time.fixedDeltaTime = Time.timeScale * .02f;

			yield return null;
		}
	}

	private void LockControl()
	{
		controlTimer = 0;
	}

	private void OnUnpause()
	{
		LockControl();
	}

}
