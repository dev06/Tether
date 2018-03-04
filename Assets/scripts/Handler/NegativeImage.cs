using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NegativeImage : MonoBehaviour {

	private Image image;
	private PlayerController player;
	private bool started = false;
	private bool started2 = true;
	private CanvasGroup group;

	Color tint;

	void OnEnable()
	{
		EventManager.OnHoldStatus += OnHoldStatus;
	}
	void OnDisable()
	{
		EventManager.OnHoldStatus -= OnHoldStatus;
	}

	void OnHoldStatus(int i)
	{
		Toggle((i == 1) ? true : false);
	}


	void Start () {
		image = GetComponent<Image>();
		player = PlayerController.Instance;
		image.material.SetColor("_Color", new Color(0, 0, 0, 0));
		group = GetComponent<CanvasGroup>();
	}

	void Update ()
	{
		if (GameplayController.GAME_STATE  != State.GAME) return;
		if (player.isHolding && !started)
		{
			StopCoroutine("ShiftColor2");
			StopCoroutine("ShiftColor");
			StartCoroutine("ShiftColor");
			started2 = false;
		}
		else if (!started2 && !player.isHolding)
		{
			StopCoroutine("ShiftColor");
			StopCoroutine("ShiftColor2");
			StartCoroutine("ShiftColor2");
			started = false;
		}
	}


	float alpha = 0;
	IEnumerator ShiftColor()
	{
		started = true;
		alpha = image.material.GetColor("_Color").a;
		while (alpha < 1f)
		{
			alpha += Time.unscaledDeltaTime * 2.25f;
			image.material.SetColor("_Color", new Color(alpha / 2f, alpha / 2f, alpha / 2f, alpha ));
			alpha = Mathf.Clamp(alpha, 0f, 1f);
			yield return null;
		}
	}

	IEnumerator ShiftColor2()
	{
		started2 = true;
		//float alpha = image.material.GetColor("_Color").a /2f;
		while (alpha > 0f)
		{
			alpha -= Time.unscaledDeltaTime * 7.25f;
			image.material.SetColor("_Color", new Color(alpha / 2f,  alpha / 2f, alpha / 2f, alpha));
			yield return null;
		}
	}

	void Toggle(bool b)
	{
		if (group == null)
		{
			group = GetComponent<CanvasGroup>();
		}

		group.alpha = b ? 1 : 0;
	}



}
