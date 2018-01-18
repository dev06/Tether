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
	private float distaceToKeep;
	private Vector3 targetLocalPosition;
	ObjectSpawner objectSpawner;
	public GameObject effect;
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
		targetLocalPosition = transform.localPosition;

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
		objectSpawner = ObjectSpawner.Instance;

		if (currentBase != null)
		{
			currentBase.player = this;
		}

		defaultScale = transform.localScale.x;
		targetPos  = transform.position;
		targetLocation = transform.localPosition;


	}


	float rotatingDir;
	[Range(.5f, 5f)]
	public float aa = 0;
	public float angle = 0;
	void Update ()
	{

		transform.right = transform.position - currentBase.transform.position;

		scale = (defaultScale / currentBase.transform.localScale.x);
		transform.localScale = new Vector3(scale, scale, scale);

		// Vector3 targetDir = objectSpawner.nextBase.transform.position - transform.position;
		// float anglea = Vector3.Angle(targetDir, transform.right);

		// if (anglea < 2f)
		// {
		// 	line.Shoot();
		// }
		//Round();

	}

	public void Round()
	{
		angle = (int)CalculateAngle(transform.position, currentBase.transform.position);
		float y = aa * Mathf.Sin(angle * Mathf.Deg2Rad);
		float xx = aa * Mathf.Cos(angle * Mathf.Deg2Rad);
		Vector3 pos = new Vector3(xx, y, 0);
		transform.localPosition  = pos;
	}

	public  float CalculateAngle(Vector3 from, Vector3 to)
	{
		return Quaternion.FromToRotation(-currentBase.transform.right, to - from).eulerAngles.z;
	}

	void OnBaseHit(BaseController hitBase, Vector2 hitPoint)
	{
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

		//BaseController.DIRECTION *= GetDirection();
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject == currentBase.transform.gameObject)
		{
			float shake = Random.Range(2.5f, 3.0f);
			float shrinkFactor = Random.Range(5.5f, 6.5f);
			Camera.main.transform.GetComponent<CameraController>().Jitter(shake, shrinkFactor);
			//Instantiate(AppResources.Effect, transform.position, Quaternion.identity);

		}

	}

	public void SetTargetLocalPosition(Vector3 position)
	{
		this.targetLocalPosition = position;
	}


	private int GetDirection()
	{
		int value = Random.Range(-1, 1);
		if (value == 0) { return 1; }
		return value;
	}


}
