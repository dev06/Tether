using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {

	public static float DIRECTION = -1f;
	public static float MIN_VELOCITY = 215f;
	public static float MAX_VELOCITY = 300F;
	public static float BASE_VELOCITY = 200f;
	public static float VELOCITY_SCALE = 1f;

	public PlayerController player;
	public Vector3 targetScale;
	public bool shouldRotate = true;
	public Transform outerRing;
	public Transform maskTransform;

	private GameplayController gameplayController;
	private float velocity;
	private float depletionRate  = .15f;
	private float minScaleThreshold = .1f;
	private Vector3 targetLocation;
	private ObjectSpawner objectSpawner;
	private bool freeze;
	private SpriteRenderer renderer;





	public void Initialize()
	{
		SetVelocity(3f);
		objectSpawner = ObjectSpawner.Instance;
		SetFreeze(true);
		renderer = GetComponent<SpriteRenderer>();
		gameplayController = GameplayController.Instance;
	}
	void Update()
	{

		// if (!freeze)
		// {
		// 	Vector3 position = transform.position;
		// 	position.x = Mathf.PingPong(Time.time, 1.5f) - 0.75f;
		// 	transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 5f);
		// }

		if (IsOutsideOfBounds())
		{
			PoolBase();
		}

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
		Vector3 bounds = Camera.main.WorldToViewportPoint(transform.position);
		if (bounds.y < -0.5f)
		{
			return true;
		}
		return false;
	}
	float alpha = 1f;
	void UpdateTransform()
	{

		if (shouldRotate)
		{
			Vector3 targetDir = objectSpawner.nextBase.transform.position - player.transform.position;
			float distance = Vector3.Angle(targetDir, player.transform.right);
			transform.Rotate(Vector3.forward, (Time.deltaTime * ((velocity * distance) * .6f + BASE_VELOCITY) * VELOCITY_SCALE)  * DIRECTION);
		}

		if (transform.localScale.x > minScaleThreshold)
		{
			//transform.localScale -= new Vector3(Time.deltaTime * depletionRate, Time.deltaTime * depletionRate, Time.deltaTime * depletionRate);
		} else
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}



	}

	public void SpawnRing(Color c)
	{
		StopCoroutine("ISpawnRing");
		StartCoroutine("ISpawnRing", c);
	}

	private IEnumerator ISpawnRing(Color c)
	{
		float offset = 1.3f;
		while (true)
		{
			outerRing.gameObject.SetActive(true);
			alpha -= Time.deltaTime ;
			Color col = new Color(c.r, c.g, c.b, alpha);
			outerRing.GetComponent<SpriteRenderer>().color = col;
			Vector3 scale = new Vector3(transform.localScale.x + offset, transform.localScale.y + offset, transform.localScale.z + offset);
			outerRing.transform.localScale = Vector3.Lerp(outerRing.transform.localScale, scale, Time.deltaTime * 3f);
			float speed = (1.7f - maskTransform.localScale.x) * Time.deltaTime;
			if (maskTransform.localScale.x < 1.9f)
			{
				maskTransform.transform.localScale += new Vector3(speed, speed, speed);
			}
			yield return null;
		}


	}

	void PoolBase()
	{
		BaseController lastBase = transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<BaseController>();
		float xRange = Random.Range(-1.15f, 2.15f);
		float yRange = Random.Range(3f, 4f);
		transform.position = lastBase.transform.position + new Vector3(xRange, yRange, 0);
		transform.SetSiblingIndex(transform.parent.childCount - 1);
		float scale = Random.Range(1f, 1.5f);
		transform.localScale = new Vector3(scale, scale, scale);
		transform.gameObject.SetActive(false);
		SetFreeze(false);
		//SetColor(Color.black);
		if (GameplayController.SCORE % 5 == 0)
		{
			Vector3 pos = (transform.parent.GetChild(transform.parent.childCount - 2).transform.position + transform.position) / 2f;
			objectSpawner.SpawnPowerup(pos);
		}
		ResetOuterRing();


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

