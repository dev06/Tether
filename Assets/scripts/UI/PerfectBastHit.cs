using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PerfectBastHit : MonoBehaviour {


	private Text text;
	private string message;

	private string[] displayMessages_t2 = {"Awesome!", "Nice!", "Fantastic!", "Way to go!", "Great!", "Amazing!", "WOAH!"};
	private string[] displayMessages_t1 = {"Perfect Hit!", "Dead on!", "On Spot!", "Bulls Eye!", "Flawless!"};
	private Image image;
	private GameplayController gpc;
	private bool hasStarted;


	private float[] defaultValues = {.06f, .45f};
	private float[] slowValues = {.1f, 1f};


	void Start ()
	{
		text = GetComponent<Text>();
		image = GetComponentInParent<Image>();
		message = "";

		if (gpc == null)
		{
			gpc = GameplayController.Instance;
		}

	}

	void OnEnable()
	{
		EventManager.OnBaseHitPerfection += OnBaseHitPerfection;
		EventManager.OnBoostStart += OnBoostStart;
	}

	void OnDisable()
	{
		EventManager.OnBaseHitPerfection -= OnBaseHitPerfection;
		EventManager.OnBoostStart -= OnBoostStart;
	}

	void Update()
	{
		if (((int)(GameplayController.SCORE)) > gpc.BestScore)
		{
			if (!hasStarted)
			{
				message = "New Highscore!";
				StopCoroutine("IType");
				StartCoroutine("IType", new float[2] {.1f, 1f});

			}

		}
	}

	void OnBoostStart()
	{
		message = "Spectacular!";
		StopCoroutine("IType");
		StartCoroutine("IType", slowValues);

	}

	void OnBaseHitPerfection(float angle)
	{
		if (gpc == null)
		{
			gpc = GameplayController.Instance;
		}


		if (gpc.inTutorial) return;

		if (angle >= 1f && angle <= 2f)
		{
			if (message.Length > 0) return;
			message = displayMessages_t2[Random.Range(0, displayMessages_t2.Length)];
			StopCoroutine("IType");
			StartCoroutine("IType", defaultValues);

		}
		else if (angle <= 1f)
		{
			if (message.Length > 0) return;
			message = displayMessages_t1[Random.Range(0, displayMessages_t1.Length)];
			StopCoroutine("IType");
			StartCoroutine("IType", defaultValues);
		}
	}

	IEnumerator IType(float[] values)
	{
		image.enabled = true;
		string msg = "";
		text.text = "";
		float delay = values[0];
		float hold = values[1];
		if (message == "New Highscore!")
		{
			hasStarted = true;
		}
		for (int i = 0; i < message.Length; i++)
		{
			msg += message[i];
			text.text = msg;
			yield return new WaitForSeconds(delay);
		}

		yield return new WaitForSeconds(hold);
		message = "";
		text.text = message;
		image.enabled = false;
	}
}
