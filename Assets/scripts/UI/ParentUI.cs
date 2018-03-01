using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ParentUI : MonoBehaviour {

	public bool showInEdit;

	protected CanvasGroup canvasGroup;

	protected GameplayController gameplayController;

	protected void Init()
	{
		canvasGroup = GetComponent<CanvasGroup>();

		gameplayController = GameplayController.Instance;
	}

	void OnValidate()
	{
		if (canvasGroup == null)
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		if (showInEdit)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	protected virtual void Show()
	{
		if (canvasGroup == null) { return; }
		canvasGroup.blocksRaycasts = true;
		canvasGroup.alpha = 1f;
	}

	protected virtual void Hide()
	{
		if (canvasGroup == null) { return; }
		canvasGroup.blocksRaycasts = false;
		canvasGroup.alpha = 0f;
	}
}
