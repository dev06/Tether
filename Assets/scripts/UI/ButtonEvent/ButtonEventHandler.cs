using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public ButtonID buttonID; 
	protected GameplayController gamePlayController; 

	protected virtual void Init()
	{
		gamePlayController = GameplayController.Instance; 
	}

	void Start()
	{
		Init(); 
	}


	public virtual void OnPointerDown(PointerEventData data)
	{
	}
	public virtual void OnPointerUp(PointerEventData data)
	{
		if(EventManager.OnButtonPress != null)
		{
			EventManager.OnButtonPress(buttonID); 
		}
	}
}

public enum ButtonID 
{
	NONE, 
	PAUSE,
	BACK,
	CREDIT,
	SETTINGS,
	MUSIC, 
	SFX,
	STARTAREA,
}
