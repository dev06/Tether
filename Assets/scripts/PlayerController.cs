using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public static PlayerController Instance;
	public BaseController currentBase;

	private LineController line;
	private int direction = -1;
	private Vector3 targetLocation = Vector3.zero;
	private float defaultScale;
	private float scale;


	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		} else
		{
			DestroyImmediate(gameObject);
		}


		targetLocation = transform.position;


	}

	void OnEnable()
	{
		EventManager.OnBaseHit += OnBaseHit;
	}

	void OnDisable() {
		EventManager.OnBaseHit -= OnBaseHit;

	}

	Vector3 targetPos;
	void Start ()
	{
		line = transform.GetChild(0).GetComponent<LineController>();

		if (currentBase != null)
		{
			currentBase.player = this;
		}

		defaultScale = transform.localScale.x;
		targetPos  = transform.position;
	}


	float rotatingDir;
	void Update ()
	{

		rotatingDir = Input.GetKey(KeyCode.D) ? rotatingDir = -160f : Input.GetKey(KeyCode.A) ? rotatingDir = 160f : 0;

		transform.right = transform.position - currentBase.transform.position;

		//transform.RotateAround(currentBase.transform.position, Vector3.forward, direction * Time.deltaTime * -160f);

		scale = (defaultScale / currentBase.transform.localScale.x);
		transform.localScale = new Vector3(scale, scale, scale);


	}

	void LateUpdate()
	{

	}

	void AttachPlayer()
	{

	}

	void OnBaseHit(BaseController hitBase, Vector2 hitPoint)
	{
		// //RaycastHit2D hit = Physics2D.Raycast(transform.position,  transform.right, Mathf.Infinity, LayerMask.GetMask("Base"));
		// //currentBase.hadPlayer = true;
		// currentBase.player = null;
		// //float scale = 1f;
		// //currentBase.targetScale = new Vector3(scale, scale, scale);
		// //currentBase.targetLocation = new Vector3(currentBase.transform.position.x, hitBase.transform.position.y + Random.Range( 3f, 4f), 0);
		// currentBase.transform.gameObject.layer = 8;
		// currentBase = hitBase;
		// transform.SetParent(currentBase.transform);
		// transform.position = new Vector3(hitPoint.x, hitPoint.y, 0);
		// currentBase.player = this;
		// currentBase.transform.gameObject.layer = 9;

		// direction *= -1;
	}

	IEnumerator PositionTranslation()
	{
		bool locked = false;
		while (!locked)
		{
			transform.position = Vector3.Lerp(transform.position, targetPos, 4f * Time.deltaTime);
			// if (Vector3.Distance(targetPosition, transform.position) < .1f)
			// {
			// 	locked = true;
			// }
			yield return null;
		}
	}

	public void SetTargetLocation(Vector2 targetLocation)
	{
		this.targetLocation = targetLocation;
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
		currentBase.transform.gameObject.layer = 9;
		direction *= -1;
	}


}
