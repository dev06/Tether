using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pause : ParentUI {


	private Color color;
	private Image image;
	void OnEnable()
	{
		EventManager.OnPause += OnPause;
		EventManager.OnUnpause += OnUnpause;
		//	EventManager.OnBoostStart +=
	}

	void OnDisable()
	{
		EventManager.OnPause -= OnPause;
		EventManager.OnUnpause -= OnUnpause;
		//EventManager.OnBoostStart+=

	}

	void Start ()
	{
		Init();
		image = GetComponent<Image>();
		IHide();
	}

	void OnPause()
	{
		if (image == null)
		{
			image = GetComponent<Image>();
		}
		image.color = GameplayController.LevelIndex == 1 ? Color.white : Color.black;
		IShow();
	}

	void OnUnpause()
	{
		if (image == null)
		{
			image = GetComponent<Image>();
		}

		image.color = GameplayController.LevelIndex == 1 ? Color.white : Color.black;
		IHide();
	}


	protected  void IShow()
	{

		StopCoroutine("DampAlpha");
		StartCoroutine("DampAlpha", 1f);
	}

	protected void IHide()
	{
		StopCoroutine("DampAlpha");
		StartCoroutine("DampAlpha", 0f);
	}

	private IEnumerator DampAlpha(float target)
	{
		float vel = 0;

		if (canvasGroup == null)
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		canvasGroup.blocksRaycasts = (target == 1);
		while (canvasGroup.alpha != target)
		{
			canvasGroup.alpha = Mathf.SmoothDamp(canvasGroup.alpha, target, ref vel, Time.unscaledDeltaTime * 5f);
			yield return null;
		}
	}
}
