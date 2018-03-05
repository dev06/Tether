using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MK.Glow;
using  UnityStandardAssets.ImageEffects;
using UnityEngine.UI;
public class CameraController : MonoBehaviour {

	public bool freezeCamera;

	public Color defaultBackgroundColor;

	public ParticleSystem menuParticleSystem;

	private ParticleSystem.ShapeModule shapeModule;



	public bool isMoving;

	private float jitterAmount;

	private float shakeAmount;

	private float decreaseFactor;

	private float vel;

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

	private float yOffset;

	private float yOffsetVel;

	private Vector3 hitOffset;

	private Twirl[] twirl;

	private float prevY;



	void OnEnable()
	{


		EventManager.OnGameStart += OnGameStart;


	}

	void OnDisable()
	{


		EventManager.OnGameStart -= OnGameStart;


	}

	void Start ()
	{


		camera = Camera.main;

		player = PlayerController.Instance;

		Camera.main.orthographicSize = 4f * Screen.height / Screen.width * 0.5f;

		spawner = ObjectSpawner.Instance;

		positions.Add(player.transform.position);

		positions.Add(spawner.transform.position);

		vortex = GetComponent<Vortex>();

		twirl = GetComponents<Twirl>();

		ToggleTwirl(false);

		shapeModule = menuParticleSystem.shape;

		menuParticleSystem.transform.position = camera.ViewportToWorldPoint(new Vector2(.5f, 0f)) + Vector3.forward;

	}

	void FixedUpdate ()
	{

		if (Input.GetKeyDown(KeyCode.R))
		{
			Time.timeScale = .1f;
			Time.fixedDeltaTime = Time.timeScale * .01f;
		}
		if (Mathf.Abs((int)transform.position.y - (int)prevY) > 0)
		{
			isMoving = true;

			prevY = transform.position.y;
		}
		else
		{
			isMoving = false;
		}

		shapeModule.radius = camera.orthographicSize / 2.0f;

		if (GameplayController.GAME_STATE != State.GAME) { return; }

		if (freezeCamera) { return; }

		if (spawner.nextBase == null) { return; }

		positions[0] = player.currentBase.transform.position;

		positions[1] = spawner.nextBase.transform.position;

		Vector2 jitter = JitterCamera();

		hitOffset = Vector3.Lerp(hitOffset, Vector2.zero, Time.deltaTime * 10f);

		yOffset = Mathf.SmoothDamp(yOffset, 0, ref yOffsetVel, Time.deltaTime * 5f);

		Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, GetDistance(), ref vel, Time.deltaTime * 15f);

		transform.position = Vector3.Lerp(transform.position, GetAveragePosition() +  new Vector3(0, 0, -10f) + new Vector3(jitter.x, jitter.y * .5f, -10f) + hitOffset, Time.unscaledDeltaTime * 3.5f);
	}

	public void StopJitter()
	{
		jitterAmount = 0;
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
		menuParticleSystem.transform.gameObject.SetActive(false);
		menuParticleSystem.Stop();
	}


	float GetDistance()
	{
		float averageSize = (player.currentBase.size + spawner.nextBase.size) / 10f;

		float slowmo = player.isHolding ? 1f : 0;

		return 	Vector2.Distance(player.currentBase.transform.position, positions[1]) + averageSize - slowmo;
	}

	Vector2 JitterCamera()
	{

		Vector2 offset = Random.insideUnitCircle * jitterAmount;

		jitterAmount -= Time.deltaTime * decreaseFactor;

		jitterAmount = Mathf.Clamp(jitterAmount, 0, jitterAmount);

		return offset;
	}
	public Animation cirlceAnimation;
	public void Twirl()
	{
		ToggleTwirl(true);
		StopCoroutine("ITwirl");
		StartCoroutine("ITwirl");
	}

	private IEnumerator ITwirl()
	{

		if (EventManager.OnTwirlActive != null)
		{
			EventManager.OnTwirlActive();
		}


		float va = .25f;
		float velocity = 1.9f;
		float limit = .8f;
		float aspect = (float)Screen.height / (float)(Screen.width);
		CameraLerpColor();
		cirlceAnimation.Play();
		cirlceAnimation.transform.gameObject.SetActive(true);
		while (va < 1.0f)
		{
			va += velocity * Time.unscaledDeltaTime;
			velocity -= Time.unscaledDeltaTime * 3f;
			velocity = Mathf.Clamp(velocity, .4f, velocity);

			va = Mathf.Clamp(va, 0f, 1f);
			twirl[0].radius = new Vector2(va * aspect, va) * limit;
			twirl[0].angle = (va * 360f);
			twirl[0].angle = Mathf.Clamp(twirl[0].angle, 0f, 360f);

			twirl[1].radius = new Vector2(va * aspect, va) * limit;
			twirl[1].angle =  360 - (va * 360f);
			twirl[1].angle = Mathf.Clamp(twirl[1].angle, 0f, 360f);

			yield return new WaitForSeconds(Time.unscaledDeltaTime);
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
		defaultBackgroundColor = camera.backgroundColor;
		camera.backgroundColor = Color.white;

		while (camera.backgroundColor != defaultBackgroundColor)
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

	private void ToggleTwirl(bool b)
	{
		twirl[0].enabled = twirl[1].enabled = b;
	}


}
