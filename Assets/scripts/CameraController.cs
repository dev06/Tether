using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	PlayerController player;
	private List<Vector3> positions = new List<Vector3>();
	ObjectSpawner spawner;
	void Start ()
	{
		player = PlayerController.Instance;
		Camera.main.orthographicSize = 4f * Screen.height / Screen.width * 0.5f;
		spawner = ObjectSpawner.Instance;
		positions.Add(player.transform.position);
		positions.Add(spawner.transform.position);
	}

	// Update is called once per frame
	float vel;
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			jitterAmount = shakeAmount;
		}
		positions[0] = player.transform.position;
		positions[1] = spawner.nextBase.transform.position;
		Vector2 jitter = JitterCamera();
		//transform.position = Vector3.Lerp(transform.position, GetAveragePosition() +  new Vector3(0, 0, -10f)  + new Vector3(jitter.x, jitter.y, transform.position.z), Time.deltaTime * 4.0f);
		Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, GetDistance(), ref vel, Time.deltaTime * 15f);
		transform.position = Vector3.Lerp(transform.position, GetAveragePosition() +  new Vector3(0, 0, -10f) + new Vector3(jitter.x, jitter.y, -10f), Time.deltaTime * 4.0f);
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

	float GetDistance()
	{
		return 	Vector2.Distance(player.currentBase.transform.position, positions[1]);
	}

	float xJitter;
	float yJitter;
	float jitterAmount;
	float shakeAmount;
	float decreaseFactor;
	float xVel = .01f;
	float yVel = .01f;
	public float xSpeed = 2.4f;

	public float ySpeed = 3.1f;
	public float xAmp = .5f;
	public float yAmp = .5f;

	Vector2 JitterCamera()
	{
		// xJitter = Mathf.PingPong(Time.time * xSpeed, xVel) * xAmp;
		// yJitter = Mathf.PingPong(Time.time * ySpeed, yVel) * yAmp;
		// xVel -= .1f;
		// yVel -= .1f;
		// if (xVel < 0) xVel = .01f;
		// if (yVel < 0) yVel = .01f;


		Vector2 offset = Random.insideUnitCircle * jitterAmount;
		jitterAmount -= Time.deltaTime * decreaseFactor;
		jitterAmount = Mathf.Clamp(jitterAmount, 0, jitterAmount);
		return offset;
	}

	public void Jitter(float amount, float decreaseFactor)
	{
		jitterAmount = amount;
		this.decreaseFactor = decreaseFactor;
	}

}
