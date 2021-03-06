﻿using System.Collections;
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

				case ButtonID.TUTORIAL:
				{
					GameplayController.TutorialEnabled = !GameplayController.TutorialEnabled;

					if (EventManager.OnTutorialActive != null)
					{
						EventManager.OnTutorialActive(GameplayController.TutorialEnabled);
					}

					break;
				}


				case ButtonID.HAPTIC:
				{
					GameplayController.HAPTIC = !GameplayController.HAPTIC;

					if (EventManager.OnHapticVibrate != null)
					{
						EventManager.OnHapticVibrate(GameplayController.HAPTIC);
					}
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
		if (GameplayController.GAME_STATE == State.MENU)
		{
			if (buttonID == ButtonID.STARTAREA)
			{
				if (!ShopPanel.ACTIVE)
				{
					mouseUp = data.position;

					float mag = Mathf.Abs(mouseUp.x - mouseDown.x);

					if (mag < Screen.width * .1f)
					{
						FindObjectOfType<LevelSelectUI>().StartGame();
					}
				}
				else
				{
					FindObjectOfType<ShopPanel>().Close();
				}
			}

		}

		switch (buttonID)
		{
			case ButtonID.PAUSEMENU:
			{
				if (EventManager.OnGameOver != null)
				{
					EventManager.OnGameOver();
				}
				break;
			}

			case ButtonID.SHOP:
			{
				FindObjectOfType<ShopButtonHandler>().ShopPanel.Open();
				break;
			}

			case ButtonID.TETHER_HEAD:
			{
				if (EventManager.OnTetherHeadClick != null)
				{
					TetherHead t = GetComponent<TetherHead>();
					EventManager.OnTetherHeadClick(t);
				}
				break;
			}

		}


	}
}
