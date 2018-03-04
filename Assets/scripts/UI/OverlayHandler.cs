using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverlayHandler : MonoBehaviour {

	CanvasGroup group;
	void Start ()
	{
		//Toggle(false);
	}

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


	void Toggle(bool b)
	{
		if (group == null)
		{
			group = GetComponent<CanvasGroup>();
		}

		group.alpha = b ? 1 : 0;
		group.blocksRaycasts = b;
	}
}
