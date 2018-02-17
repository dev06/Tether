using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI; 
public class SimpleButtonHandler : ButtonEventHandler {


	protected override void Init()
	{
		base.Init(); 
	}

	public override void OnPointerUp(PointerEventData data)
	{
		base.OnPointerUp(data); 

		try
		{
			switch(buttonID)
			{
				case ButtonID.PAUSE: 
				{

					gamePlayController.TogglePause(!gamePlayController.Paused); 
					break; 
				}

				case ButtonID.BACK: 
				{
					GameplayController.SetState(State.MENU); 
					break; 
				}

				case ButtonID.CREDIT: 
				{
					GameplayController.SetState(State.CREDIT); 
					break; 
				}

				case ButtonID.STARTAREA: 
				{
					FindObjectOfType<LevelSelectUI>().StartGame();  
					break; 
				}
			}
		}
		catch(System.Exception e)
		{
			Debug.LogError(e); 
		}
	}
}
