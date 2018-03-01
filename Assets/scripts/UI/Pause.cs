using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : ParentUI {


	void OnEnable()
	{
		EventManager.OnPause+=OnPause; 
		EventManager.OnUnpause+=OnUnpause; 
	}

	void OnDisable()
	{
		EventManager.OnPause-=OnPause; 
		EventManager.OnUnpause-=OnUnpause; 

	}

	void Start ()
	{
		Init();
		IHide(); 
	}

	void OnPause()
	{
		IShow(); 
	}

	void OnUnpause()
	{
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
		if(canvasGroup == null)
		{
			canvasGroup = GetComponent<CanvasGroup>(); 
		}
		canvasGroup.blocksRaycasts = (target == 1); 
		while(canvasGroup.alpha != target)
		{
			canvasGroup.alpha = Mathf.SmoothDamp(canvasGroup.alpha, target, ref vel, Time.unscaledDeltaTime * 2f); 
			yield return new WaitForSeconds(Time.unscaledDeltaTime);
		}
	}
}
