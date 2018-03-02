using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {


	public static float DIRECTION = -1f;

	public static float MIN_VELOCITY = 215f;

	public static float MAX_VELOCITY = 300F;

	public static float BASE_VELOCITY = 200f;

	public static float VELOCITY_SCALE = 1f;

	public static float DEFAULT_MIN_SCALE = 1.1F;

	public static float DEFAULT_MAX_SCALE = 3f;



	public PlayerController player;

	public float size;

	public Vector3 targetScale;

	public bool shouldRotate = true;

	public bool visited;

	public Transform outerRing;

	public Transform maskTransform;



	private GameplayController gameplayController;

	private float velocity;

	private float default_depletion_rate = .1f;

	private float depletionRate;

	private float minScaleThreshold = .1f;

	private float alpha = 1f;

	private Vector3 targetLocation;

	private ObjectSpawner objectSpawner;

	private bool freeze;

	private SpriteRenderer renderer;

	private ParticleSystem zap;

	public void Initialize()
	{

		SetVelocity(3f);

		objectSpawner = ObjectSpawner.Instance;

		SetFreeze(true);

		renderer = GetComponent<SpriteRenderer>();

		gameplayController = GameplayController.Instance;

		zap = GameObject.Find("Zap").GetComponent<ParticleSystem>();

		depletionRate = default_depletion_rate;
	}
	void Update()
	{
		if (GameplayController.GAME_STATE == State.PAUSE) { return; }

		if (GameplayController.GAME_STATE != State.GAME) { return; }

		//		renderer.material.SetColor("Glow Color", renderer.color);

		if (player == null && visited)
		{
			zap.transform.position = transform.position;
			zap.startColor = renderer.color;
			zap.Play();
			PoolBase();

		}

		// if (Input.GetKeyDown(KeyCode.Y))
		// {
		// 	zap.transform.position = transform.position;
		// 	zap.startColor = renderer.color;
		// 	zap.Play();
		// }


		if (player == null)
		{
			gameObject.layer = 8;
			ResetOuterRing();
			return;
		}

		UpdateTransform();

	}

	public void SetColor(Color c)
	{
		this.renderer.color = c;
	}

	bool IsOutsideOfBounds()
	{


		float offset = .5f * transform.localScale.y;

		Vector3 topPoint = new Vector3(transform.position.x, transform.position.y + offset, 0);

		Vector3 bounds = Camera.main.WorldToViewportPoint(topPoint);

		if (bounds.y < 0.0f)
		{
			return true;
		}

		return false;
	}
	void UpdateTransform()
	{
		if (GameplayController.SCORE == 0)
		{
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
			shouldRotate = false;
		}

		if (shouldRotate)
		{

			Vector3 targetDir = objectSpawner.nextBase.transform.position - player.transform.position;

			float distance = Vector3.Angle(targetDir, player.transform.right);

			transform.Rotate(Vector3.forward, (Time.deltaTime * ((velocity * distance) * .6f + BASE_VELOCITY) * VELOCITY_SCALE)  * DIRECTION);

		}

		if (gameplayController.inTutorial == false)
		{
			default_depletion_rate += Time.unscaledDeltaTime * GameplayController.DIFFICULTY * .0017f;
		}

		if (transform.localScale.x > minScaleThreshold)
		{
			depletionRate = player.isHolding ? .3f : default_depletion_rate;

			if (!gameplayController.DEBUG)
			{
				float dv = depletionRate * Time.unscaledDeltaTime;

				if (!gameplayController.inTutorial)
				{
					transform.localScale -= new Vector3(dv, dv, dv);
				}
			}
		}
		else
		{
			if (EventManager.OnGameOver != null)
			{
				EventManager.OnGameOver();
			}

			//UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}

		size = transform.localScale.sqrMagnitude;

	}

	public void SpawnRing(Color c)
	{
	}

	private IEnumerator ISpawnRing(Color c)
	{
		float offset = 1.3f;

		SpriteRenderer outerRingColor = outerRing.GetComponent<SpriteRenderer>();

		while (player != null)
		{

			outerRing.gameObject.SetActive(true);

			alpha -= Time.unscaledDeltaTime ;

			Color col = new Color(c.r, c.g, c.b, alpha);

			outerRingColor.color = col;

			Vector3 scale = new Vector3(transform.localScale.x + offset, transform.localScale.y + offset, transform.localScale.z + offset);

			outerRing.transform.localScale = Vector3.Lerp(outerRing.transform.localScale, scale, Time.unscaledDeltaTime * 3f);

			float speed = (1.7f - maskTransform.localScale.x) * Time.unscaledDeltaTime;

			if (maskTransform.localScale.x < 1.9f)
			{
				maskTransform.transform.localScale += new Vector3(speed, speed, speed);
			}
			yield return null;
		}


	}

	private void PoolBase()
	{

		BaseController lastBase = transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<BaseController>();


		float xRange = Random.Range(-1.35f, 2.35f) * GameplayController.DIFFICULTY * .2f;

		xRange = Mathf.Clamp(xRange, -3.5f, 3.5f);

		Vector2 yRangeValues = new Vector2(7f, 9f);

		float yRange = Random.Range(yRangeValues.x, yRangeValues.y) * GameplayController.DIFFICULTY * .2F;

		yRange = Mathf.Clamp(yRange, yRangeValues.x, yRangeValues.y);

		transform.position = lastBase.transform.position + new Vector3(xRange, yRange, 0);

		transform.SetSiblingIndex(transform.parent.childCount - 1);

		float scale = DEFAULT_MAX_SCALE - (GameplayController.DIFFICULTY * .2F);

		scale = Mathf.Clamp(scale, DEFAULT_MIN_SCALE, DEFAULT_MAX_SCALE);

		transform.localScale = new Vector3(scale, scale, scale);

		transform.gameObject.SetActive(false);

		transform.rotation = Quaternion.Euler(Vector3.zero);

		SetFreeze(false);

		if (GameplayController.SCORE % 6 == 0)
		{

			Vector3 pos = (transform.parent.GetChild(transform.parent.childCount - 2).transform.position + transform.position) / 2f;

			objectSpawner.SpawnPowerup(pos);
		}

		visited = false;



		//	ResetOuterRing();


	}

	private void ResetOuterRing()
	{

		outerRing.transform.localScale = new Vector3(1, 1, 1);

		maskTransform.localScale = new Vector3(1, 1, 1);

		outerRing.gameObject.SetActive(false);

		alpha = 1f;

	}

	public void SetFreeze(bool f)
	{

		this.freeze = f;
	}
	public void SetVelocity(float vel)
	{

		this.velocity = vel;
	}

	public void SetTargetLocation(Vector3 location)
	{
		this.targetLocation = location;
	}

	public void SetTargetScale(Vector3 scale)
	{
		this.targetScale = scale;
	}
}

