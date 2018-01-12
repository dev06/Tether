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
	}
	bool b;
	bool attached = false;
	bool endLineToPlayer = true;
	void Update ()
	{
		transform.right = player.transform.right;
		if (Input.GetMouseButtonDown(0) && !attached)
		{

			//if (active) { return; }
			//StopCoroutine("Shoot");
			//	StartCoroutine("Shoot");
			float length = 5f;
			RaycastHit2D hit = Physics2D.Raycast(transform.position,  transform.right, length, LayerMask.GetMask("Base"));
			startLinePosition = player.transform.position;
			endLinePosition = transform.position + (transform.right * length);
			//line.SetPosition(1, endLinePosition);
			endLineToPlayer = false;
			if (hit.collider != null)
			{

				hitPoint = hit.point;
				endLinePosition = hit.point;
				// if (EventManager.OnBaseHit != null)
				// {
				// 	EventManager.OnBaseHit(hit.transform.GetComponent<BaseController>(), hit.point);
				// }
				object[] objs = new object[2] {hit.transform.GetComponent<BaseController>(), hit.point};
				StopCoroutine("Retract");
				StopCoroutine("Attach");
				StartCoroutine("Attach", objs);
				//line.SetPosition(1, player.transform.position);

			}

		}

		Debug.DrawRay(transform.position,  (transform.right * length));

		//	endLinePosition = Vector3.Lerp(endLinePosition, startLinePosition, Time.deltaTime * 5f);
		//startLinePosition = Vector3.Lerp(startLinePosition, , Time.deltaTime * 15f);

		if (!attached)
		{
			startLinePosition = player.transform.position;
		}

		if (endLineToPlayer)
		{
			endLinePosition = player.transform.position;
		}
		line.SetPosition(0, startLinePosition);
		line.SetPosition(1, endLinePosition);

		collider.transform.position = line.GetPosition(1);

		if (IsOutsideOfBounds())
		{

			StopCoroutine("Retract");
			StartCoroutine("Retract");
		}

	}
	bool IsOutsideOfBounds()
	{
		Vector3 bounds = Camera.main.WorldToScreenPoint(endLinePosition);
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
		}

		player.currentBase.shouldRotate = false;
		BaseController.DIRECTION *= -1;
		endLineToPlayer = false;
		yield return new WaitForSeconds(.15f);
		while (Mathf.Abs(Vector2.Distance(hitPosition, startLinePosition)) > .01f)
		{
			b = false;
			startLinePosition = Vector3.Lerp(startLinePosition, hitPosition, Time.deltaTime * 15f);
			player.SetLocation(startLinePosition);
			yield return null;
		}

		attached = false;
		player.currentBase.shouldRotate = true;
		endLineToPlayer = true;
		b = true;
	}

	IEnumerator Retract()
	{
		while (Mathf.Abs(Vector2.Distance(endLinePosition, player.transform.position)) > .05f)
		{
			endLinePosition = Vector3.Lerp(endLinePosition, player.transform.position, Time.deltaTime * 10f);
			yield return null;
		}

		endLineToPlayer = true;
	}

	IEnumerator Shoot()
	{
		bool locked = true;

		float timer = 0 ;

		float direction = 0;

		float dv = 0;

		float target = 0;

		while (locked)
		{
			active = true;
			dv = direction;

			timer += Time.deltaTime;

			direction = Mathf.PingPong(timer, 1);

			float diff = direction - dv;

			target = diff > 0 ? length : diff < 0 ? -length : 0;

			collider.transform.gameObject.SetActive(diff > 0);

			if (direction < .05f && diff < 0)
			{
				active = false;

				locked = false;

			}

			endLinePosition += new Vector3(target, 0, 0) * Time.deltaTime  * speed;


			float cap = endLinePosition.x < 0 ? 0 : endLinePosition.x;

			endLinePosition = new Vector3(cap, endLinePosition.y, endLinePosition.z);

			line.SetPosition(1, endLinePosition);

			yield return null;
		}

		endLinePosition = Vector3.zero;

		line.SetPosition(1, endLinePosition);
	}



	IEnumerator IContractLine()
	{

		while (line.GetPosition(1).magnitude > .1f)
		{
			Vector3 position = Vector3.Lerp(line.GetPosition(1), Vector3.zero, Time.deltaTime * 3f);
			line.SetPosition(1, position);
			yield return null;
		}
		ResetLine();
	}

	public void ContractLine()
	{
		StopCoroutine("Shoot");
		StopCoroutine("IContractLine");
		StartCoroutine("IContractLine");
	}


	public void ResetLine() {

		// StopCoroutine("Shoot");
		// active = false;
		// endLinePosition = Vector3.zero;
		// line.SetPosition(1, endLinePosition);

	}





}
