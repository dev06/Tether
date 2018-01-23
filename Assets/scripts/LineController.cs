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
	public float length = 2.0f;

	private float time = 1f;

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
		objectSpawner = ObjectSpawner.Instance;
		gameplayController = GameplayController.Instance;
	}

	float timeVel;

	void Update ()
	{
		if (!player.activeBoost)
		{
			Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 1f, ref timeVel, Time.unscaledDeltaTime * 20f);
			Time.fixedDeltaTime = Time.timeScale * .02f;
		}
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
	public  float CalculateAngle(Vector3 from, Vector3 to)
	{
		return Quaternion.FromToRotation(from, to ).eulerAngles.z;
	}
	public LayerMask layer, allHit;
	public void Shoot()
	{
		if (attached) { return; }

		float length = 8f;

		RaycastHit2D hit = Physics2D.Raycast(transform.position,  transform.right, length, layer);

		RaycastHit2D all = Physics2D.Raycast(transform.position,  transform.right, length, allHit);

		if (all.collider != null)
		{
			if (all.transform.gameObject.tag == "Objects/powerup" && !player.activeBoost)
			{
				player.activeBoost = true;
				Camera.main.GetComponent<CameraController>().StartVortex();
				all.transform.gameObject.SetActive(false);
				player.bsm.Activate();
			}
		}

		startLinePosition = player.transform.position;

		endLinePosition = transform.position + (transform.right * length);

		endLineToPlayer = false;

		hitSomething = (hit.collider != null);

		line.startWidth = line.endWidth = defaultLineWidth;

		if (hit.collider != null)
		{

			float a = CalculateAngle(hit.point, hit.transform.position);

			BaseController.DIRECTION = (a > 0 && a < 180) ? 1 : -1;

			Particle effect = player.effect.GetComponent<Particle>();

			effect.SetPosition(hit.point);

			effect.Play();


			hitPoint = hit.point;

			endLinePosition = hit.point;

			line.endWidth = .25f;


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
			gameplayController.IncrementScore();
			hitBase.SetFreeze(true);
		} else
		{
			if (!player.activeBoost)
			{
				Camera.main.GetComponent<CameraController>().freezeCamera = true;
				Time.timeScale = .2f;
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

		if (Camera.main.GetComponent<CameraController>().freezeCamera)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
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
