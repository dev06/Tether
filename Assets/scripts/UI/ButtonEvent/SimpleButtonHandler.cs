using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SimpleButtonHandler : ButtonEventHandler {


	private Vector2 mouseDown;
	private Vector2 mouseUp;

	protected override void Init()
	{
		base.Init();
	}

	public override void OnPointerUp(PointerEventData data)
	{
		if (buttonID == ButtonID.BACK)
		{
			GameplayController.SetState(State.MENU);

		}
	}

	public override void OnPointerDown(PointerEventData data)
	{
		base.OnPointerDown(data);

		mouseDown = data.position;
		try
		{
			switch (buttonID)
			{
				case ButtonID.PAUSE:
				{

					gamePlayController.TogglePause(!gamePlayController.Paused);
					break;
				}

				case ButtonID.PAUSEMENU:
				{
					if (EventManager.OnGameOver != null)
					{
						EventManager.OnGameOver();
					}
					break;
				}

				case ButtonID.VAYSTUDIOS:
				{
					Application.OpenURL("https://www.vaystudios.com");
					break;
				}



				case ButtonID.CREDIT:
				{
					GameplayController.SetState(State.CREDIT);
					break;
				}

				case ButtonID.MUSIC:
				{
					AudioController.Mute = !AudioController.Mute;

					if (EventManager.OnMute != null)
					{
						EventManager.OnMute(AudioController.Mute);
					}
					break;
				}


				case ButtonID.REVERB:
				{
					AudioController.Instance.ToggleReverb(!AudioController.ReverbOn);

					if (EventManager.OnReverb != null)
					{
						EventManager.OnReverb(AudioController.ReverbOn);
					}
					break;
				}
				case ButtonID.SETTINGS:
				{
					// /GameplayController.SetState(State.SETTING);
					break;
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError(e);
		}
	}

	public override void OnPointerClick(PointerEventData data)
	{
		//base.OnPointerClick(data);
		if (GameplayController.GAME_STATE == State.MENU)
		{
			if (buttonID == ButtonID.STARTAREA)
			{
				mouseUp = data.position;

				float mag = Mathf.Abs(mouseUp.x - mouseDown.x);

				if (mag < 20)
				{
					FindObjectOfType<LevelSelectUI>().StartGame();
				}
			}
		}

	}
}
