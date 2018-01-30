using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class ParentUI : MonoBehaviour {

	protected CanvasGroup canvasGroup; 

	protected GameplayController gameplayController;

	protected void Init()
	{
		canvasGroup = GetComponent<CanvasGroup>(); 

		gameplayController = GameplayController.Instance; 
	}

	protected void Show()
	{
		if(canvasGroup == null) return; 
		canvasGroup.blocksRaycasts = true; 
		canvasGroup.alpha = 1f; 
	}

	protected void Hide()
	{
		if(canvasGroup == null) return; 
		canvasGroup.blocksRaycasts = false; 
		canvasGroup.alpha = 0f; 
	}
}
