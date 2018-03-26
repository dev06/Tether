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
	private PlayerController player; 
	private bool hasStarted;


	private float[] defaultValues = {.06f, .45f, 0f};
	private float[] slowValues = {.1f, 1f, 0f};


	void Start ()
	{
		text = GetComponent<Text>();
		image = GetComponentInParent<Image>();
		message = "";

		if (gpc == null)
		{
			gpc = GameplayController.Instance;
		}

		if(player == null)
		{
			player = PlayerController.Instance; 
		}

		//PlayerPrefs.DeleteAll();

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

	bool b1;
	bool b2;
	bool b3;
	bool b4;
	bool b5;
	void Update()
	{
		if (GameplayController.GAME_STATE != State.GAME) { return; }
		if (((int)(GameplayController.SCORE)) > gpc.BestScore && gpc.inTutorial == false)
		{
			if (!hasStarted)
			{

				message = "New Highscore!";
				StopCoroutine("IType");
				StartCoroutine("IType", new float[3] {.1f, 1f, 0f});
				hasStarted = true;
			}
		}

		if (GameplayController.TutorialEnabled)
		{
			if ((int)(GameplayController.SCORE) == 0)
			{
				if (!b1)
				{
					Hide();
					message = "Tap to shoot the tether!";
					StopCoroutine("IType");
					StartCoroutine("IType", new float[3] {.04f, 2f, .5f});
					b1 = true;
				}
			} 
			else if ((int)(GameplayController.SCORE) == 1)
			{
				if (!b2)
				{
					Hide();
					message = "Tap and Hold for Slow Motion!";
					StopCoroutine("IType");
					StartCoroutine("IType", new float[3] {.04f, 2f, 0f});
					b2 = true;
				}
			}
			else if ((int)(GameplayController.SCORE) == 2)
			{
				if (!b3)
				{
					Hide();
					message = "Avoid The Boundaries!";
					StopCoroutine("IType");
					StartCoroutine("IType", new float[3] {.04f, 2f, 0f});
					b3 = true;
				}
			} 
			else if ((int)(GameplayController.SCORE) == 3)
			{
				if (!b4)
				{
					message = "Good Luck!";
					StopCoroutine("IType");
					StartCoroutine("IType", new float[3] {.04f, 1f, 0f});
					b4 = true;
				}
			} 
			else
			{
				if (!b5)
				{
					Hide();
					image.enabled = false;
					StopCoroutine("IType");
					b5 = true;
				}
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


		if ((int)GameplayController.SCORE <= 3) { return; }

		if (angle >= 1f && angle <= 2f)
		{
			if (message.Length > 0) { return; }
			message = displayMessages_t2[Random.Range(0, displayMessages_t2.Length)];
			StopCoroutine("IType");
			StartCoroutine("IType", defaultValues);
			Haptic.Vibrate(HapticIntensity.Light);


		}
		else if (angle <= 1f)
		{
			if (message.Length > 0) { return; }
			message = displayMessages_t1[Random.Range(0, displayMessages_t1.Length)];
			StopCoroutine("IType");
			StartCoroutine("IType", defaultValues);
			Haptic.Vibrate(HapticIntensity.Light);

		}
	}

	IEnumerator IType(float[] values)
	{
		
		text.enabled = true;
		string msg = "";
		text.text = "";
		float delay = values[0];
		float hold = values[1];
		float startindDelay = values[2];
		yield return new WaitForSeconds(startindDelay);
		image.enabled = true;
		for (int i = 0; i < message.Length; i++)
		{
			msg += message[i];
			text.text = msg;
			yield return new WaitForSeconds(delay);
		}

		yield return new WaitForSeconds(hold);
		Hide();
		image.enabled = false;
	}

	void Hide()
	{
		message = "";
		text.text = "";
		text.enabled = false;

	}
}
