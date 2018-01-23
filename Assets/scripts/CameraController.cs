using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MK.Glow;
using  UnityStandardAssets.ImageEffects;
public class CameraController : MonoBehaviour {

	public bool freezeCamera;

	private float jitterAmount;
	private float shakeAmount;
	private float decreaseFactor;
	private float vel;
	private float glowVel;
	private float def_glowintensity;
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


	void Start ()
	{
		camera = Camera.main;
		player = PlayerController.Instance;
		Camera.main.orthographicSize = 4f * Screen.height / Screen.width * 0.5f;
		spawner = ObjectSpawner.Instance;
		positions.Add(player.transform.position);
		positions.Add(spawner.transform.position);
		mkglow = transform.GetComponent<MKGlowFree>();
		def_glowintensity = mkglow.GlowIntensityInner;
		vortex = GetComponent<Vortex>();
	}

	void FixedUpdate ()
	{
		if (freezeCamera) return;
		if (spawner.nextBase == null) return;
		positions[0] = player.transform.position;
		positions[1] = spawner.nextBase.transform.position;
		Vector2 jitter = JitterCamera();
		Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, GetDistance(), ref vel, Time.deltaTime * 15f);
		transform.position = Vector3.Lerp(transform.position, GetAveragePosition() +  new Vector3(0, 0, -10f) + new Vector3(jitter.x, jitter.y, -10f), Time.deltaTime * 4.0f);
		mkglow.GlowIntensityInner = Mathf.SmoothDamp(mkglow.GlowIntensityInner, def_glowintensity, ref glowVel, Time.deltaTime * 35f);
		vortexCenterPosition = Mathf.SmoothDamp(vortexCenterPosition, 2, ref vortexVel, Time.deltaTime * 80f);
		vortex.center = new Vector2(.5f, vortexCenterPosition);
		vortex.angle = player.activeBoost ? Mathf.PingPong(Time.time * 20f, vortexAngle) - vortexAngle / 2f : 0f;
		if (Input.GetKeyDown(KeyCode.C))
		{
			StartVortex();
		}
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


	Color GetColor()
	{
		float hue = Mathf.PingPong(Time.time / 25f, 1.0f);
		float sat = Mathf.PingPong(Time.time / 25f, 1.0f);
		float bright = Mathf.PingPong(Time.time / 25f, 1.0f);
		return Color.HSVToRGB(hue, sat, bright);
	}

	float GetDistance()
	{
		return 	Vector2.Distance(player.currentBase.transform.position, positions[1]);
	}

	Vector2 JitterCamera()
	{
		Vector2 offset = Random.insideUnitCircle * jitterAmount;
		jitterAmount -= Time.deltaTime * decreaseFactor;
		jitterAmount = Mathf.Clamp(jitterAmount, 0, jitterAmount);
		return offset;
	}

	public void StartVortex()
	{
		vortexCenterPosition = -1;
		vortexAngle = 30f;
	}

	public void Jitter(float amount, float decreaseFactor)
	{
		jitterAmount = amount;
		this.decreaseFactor = decreaseFactor;
	}

	public void SetGlow(float g)
	{
		mkglow.GlowIntensityInner = g;
	}

	public Color GetBackgroundNegative()
	{
		float v = 1f;
		return new Color(v - camera.backgroundColor.r, v - camera.backgroundColor.g, v - camera.backgroundColor.b, 1);
	}

}
