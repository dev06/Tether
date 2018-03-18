using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;
public class ShopPanel : ButtonEventHandler {

	public static bool ACTIVE;
	private CanvasGroup canvasGroup;
	void Start ()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		Close();
	}

	public void Open()
	{
		canvasGroup.alpha = 1f;
		canvasGroup.blocksRaycasts = true;
		ACTIVE = true;
	}

	public void Close()
	{
		canvasGroup.alpha = 0f;
		canvasGroup.blocksRaycasts = false;
		ACTIVE = false;
	}

	public override void OnPointerClick(PointerEventData data)
	{
		if (GameplayController.GAME_STATE == State.MENU)
		{
			if (ACTIVE)
			{
				if (FindObjectOfType<LevelSelectUI>().Displacement.x <= 2)
				{
					Close();
				}
			}
		}
	}
}
