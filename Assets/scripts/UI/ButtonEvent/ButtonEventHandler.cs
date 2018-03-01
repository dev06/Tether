using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {

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
		if (EventManager.OnButtonPress != null)
		{
			EventManager.OnButtonPress(buttonID);
		}
	}

	public virtual void OnPointerClick(PointerEventData data)
	{
		if (buttonID == ButtonID.STARTAREA)
		{
			Debug.Log(buttonID);

			if (EventManager.OnButtonPress != null)
			{
				EventManager.OnButtonPress(buttonID);
			}
		}
	}
	public virtual void OnPointerUp(PointerEventData data)
	{
		// if (EventManager.OnButtonPress != null)
		// {
		// 	EventManager.OnButtonPress(buttonID);
		// }
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
	SOUNDCLOUD,
	FACEBOOK,
	YOUTUBE,
	TWITTER,
	PAUSEMENU,
	REVERB,
}


