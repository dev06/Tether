using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SocialLinks : ButtonEventHandler {



	protected override void Init()
	{
		base.Init();
	}

	public override void OnPointerClick(PointerEventData data)
	{
		OpenLink(buttonID);
	}

	private void OpenLink(ButtonID id)
	{
		switch (id)
		{
			case ButtonID.SOUNDCLOUD:
			{
				Application.OpenURL("https://www.soundcloud.com/sauniks");
				return;
			}
			case ButtonID.FACEBOOK:
			{
				Application.OpenURL("https://www.facebook.com/sauniksofficial");
				return;
			}
			case ButtonID.YOUTUBE:
			{
				Application.OpenURL("https://www.youtube.com/sauniksofficial");
				return;
			}
			case ButtonID.TWITTER:
			{
				Application.OpenURL("https://www.twitter.com/sauniks");
				return;
			}
		}
	}
}
