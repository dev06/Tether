using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour {

	public static LineController Instance;



	public LineRenderer line;
	Vector3 endLinePosition;
	Vector3 startLinePosition;
	public float length = 2.0f;
	private bool active;
	public float speed = 3.0f;
	public PlayerController player;
	private float defaultLineWidth = .05f;

	public LineCollider collider;
	public GameObject currentBase;
	public Vector3 hitPoint;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		} else
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
	}

	bool attached = false;
	bool endLineToPlayer = true;
	float retractTimer = 0;
	bool hitSomething = false;
	float lineWidthVel;
	void Update ()
	{
		transform.right = player.transform.right;
		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}


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


	}

	public void Shoot()
	{
		if (attached) { return; }

		float length = 5f;

		RaycastHit2D hit = Physics2D.Raycast(transform.position,  transform.right, length, LayerMask.GetMask("Base"));

		startLinePosition = player.transform.position;

		endLinePosition = transform.position + (transform.right * length);

		endLineToPlayer = false;

		hitSomething = (hit.collider != null);

		line.startWidth = line.endWidth = defaultLineWidth;

		if (hit.collider != null)
		{
			Particle effect = player.effect.GetComponent<Particle>();

			effect.SetPosition(hit.point);

			effect.Play();

			hitPoint = hit.point;

			endLinePosition = hit.point;

			line.endWidth = .2f;

			Vector2 dir = hit.point - (Vector2)hit.transform.position;

			float angle = Vector2.Angle(dir, hit.transform.right);

			BaseController.DIRECTION = (angle > 90) ? -1 : 1;

			object[] objs = new object[2] {hit.transform.GetComponent<BaseController>(), hit.point};

			StopCoroutine("Retract");

			StopCoroutine("Attach");

			StartCoroutine("Attach", objs);
		}

		if (!hitSomething)
		{
			StopCoroutine("Retract");

			StartCoroutine("Retract");
		}
	}

	bool IsOutsideOfBounds()
	{
		Vector3 bounds = Camera.main.WorldToScreenPoint(line.GetPosition(1));
		if (bounds.x > Screen.width || bounds.x < 0 || bounds.y > Screen.height || bounds.y < 0)
		{
			return true;
		}
		return false;
	}
	IEnumerator Attach(object[] objs)
	{
		BaseController hitBase = (BaseController)objs[0];
		Vector2 hitPosition = (Vector2)objs[1];
		attached = true;

		if (hitBase != null)
		{
			player.SetTargetBase(hitBase);
			//ObjectSpawner.Instance.PositionNextBase();

			//	ObjectSpawner.Instance.CalculateDirection();
		}

		player.currentBase.shouldRotate = false;
		endLineToPlayer = false;
		yield return new WaitForSeconds(.15f);
		hitSomething = false;


		while (Mathf.Abs(Vector2.Distance(hitPosition, startLinePosition)) > .01f)
		{

			startLinePosition = Vector3.Lerp(startLinePosition, hitPosition, Time.deltaTime * 30f);
			player.SetLocation(startLinePosition);
			yield return null;
		}

		attached = false;
		player.currentBase.shouldRotate = true;
		endLineToPlayer = true;
		player.currentBase.transform.gameObject.layer = 9;

		player.Round();
	}

	IEnumerator Retract()
	{

		while (Mathf.Abs(Vector2.Distance(endLinePosition, player.transform.position)) > .07f)
		{
			endLinePosition = Vector3.Lerp(endLinePosition, player.transform.position, Time.deltaTime * 10f);
			yield return null;
		}

		endLineToPlayer = true;
	}

}
