using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {

	public PlayerController player;
	float depletionRate  = .15f;

	public SpriteRenderer spriteRenderer;

	public Vector3 targetScale;
	public bool hadPlayer;

	public bool shouldRotate = true;
	public static float DIRECTION = -1f;

	private float velocity;

	private Vector3 targetLocation;

	public Vector3 targetPoint;

	public CoinGroup coinGroup;

	ObjectSpawner objectSpawner;

	void Start()
	{
		SetVelocity(3f);
		//targetLocation = transform.position;
		objectSpawner = ObjectSpawner.Instance;

		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	void Update()
	{

		if (player != null)
		{
			Vector3 targetDir = objectSpawner.nextBase.transform.position - player.transform.position;
			float distance = Vector3.Angle(targetDir, player.transform.right);


			if (shouldRotate) {
				transform.Rotate(Vector3.forward, Time.deltaTime * ((velocity * distance) * .3f + 200f) * DIRECTION);

			}
			if (transform.localScale.x > .1f) {
				//transform.localScale -= new Vector3(Time.deltaTime * depletionRate, Time.deltaTime * depletionRate, Time.deltaTime * depletionRate);
			}
		} else
		{
			gameObject.layer = 8;
		}

		//	transform.position = Vector3.Lerp(transform.position, targetLocation, Time.deltaTime * 5f);
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

